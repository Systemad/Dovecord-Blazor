using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Timers;
using Dovecord.Client.Extensions;
using Dovecord.Client.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Dovecord.Client.Pages.Communication
{
    // TODO: maybe split up like this https://github.com/blazorhero/CleanArchitecture/blob/master/src/Client/Shared/Components/UserCard.razor.cs
    // https://github.com/blazorhero/CleanArchitecture/blob/master/src/Client/Shared/MainBody.razor.cs
    // TODO: Split user service to MainLayout for example and keep Chat purely for messaging handling
    // TODO: Add property ONLINE to user
    // TODO: MainLayout: OnConnect, take  username and Id, then send to REST (UserController) and check if user exists,
    // TODO: if it does, change ONLINE to true
    // TODO: UserService, Take all users with property ONLINE as TRUE and add it list
    public partial class Chat : IAsyncDisposable
    {
        List<ChannelMessage> _messages;
        readonly HashSet<Actor> _usersTyping = new();
        readonly HashSet<IDisposable> _hubRegistrations = new();
        readonly Timer _debounceTimer = new()
        {
            Interval = 750,
            AutoReset = false
        };

        private Channel CurrentChannel { get; set; }
        private Channel LastChannel { get; set; }
        
        HubConnection _hubConnection;

        string _messageId;
        bool _isTyping;
        private List<Channel> Channels { get; set; } = new List<Channel>();
        ActorCommand _lastCommand;
        [Parameter] public string _messageInput { get; set; }
        //List<SpeechSynthesisVoice> _voices;
        string _voice = "Auto";
        double _voiceSpeed = 1;
        
        public Chat() =>
            _debounceTimer.Elapsed +=
                async (sender, args) => await SetIsTyping(false);
        
        [Inject] public IJSRuntime JavaScript { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILogger<Chat> Log { get; set; }
        [Inject] private IChannelApi ChannelApi { get; set; }
        [Inject] private IChatApi ChatApi { get; set; }


        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }

        private string isTypingMarkup;
        private string placeholder;
        private string CurrentUsername;
        private Guid CurrentUserId;

        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"),
                    options => options.AccessTokenProvider =
                        async () => await GetAccessTokenValueAsync())
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();

            _hubRegistrations.Add(_hubConnection.OnMessageReceived(OnMessageReceivedAsync));
            _hubRegistrations.Add(_hubConnection.OnUserTyping(OnUserTypingAsync));
            _hubRegistrations.Add(_hubConnection.OnDeleteMessageReceived(OnDeleteMessageReceived));
            //_hubRegistrations.Add(
            //    _hubConnection.OnCommandSignalReceived(OnCommandSignalReceived));
            //_hubRegistrations.Add(_hubConnection.OnUserLoggedOn(
            //    actor => JavaScript.NotifyAsync("Hey!", $"{actor.User} logged on...")));
            //_hubRegistrations.Add(_hubConnection.OnUserLoggedOff(
            //    actor => JavaScript.NotifyAsync("Bye!", $"{actor.User} logged off...")));

            await _hubConnection.StartAsync();

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            CurrentUsername = user.Identity?.Name;
            CurrentUserId = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
            // TODO: Get right user ID
            
            //Log.LogInformation($"User id - {user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/nameidentifier")?.Value}");
            //Log.LogInformation($"Current userid - {authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value}");
            
            Channels = await ChannelApi.ChannelList();
            CurrentChannel = Channels.First(a => a.ChannelName == "General");
            Log.LogInformation($"Current chat id of general - {CurrentChannel.Id.ToString()}");
            await LoadChannelChat(CurrentChannel);
        }
        
        async ValueTask<string> GetAccessTokenValueAsync()
        {
            var result = await TokenProvider.RequestAccessToken();
            return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
        }

        async Task OnDeleteMessageReceived(Guid messageId) => await InvokeAsync(async () =>
        {
            _messages.Remove(_messages.First(a => a.Id == messageId));
            await JavaScript.ScrollIntoViewAsync();
            StateHasChanged();
        });

        private async Task OnMessageReceivedAsync(ChannelMessage message) =>
            await InvokeAsync(
                async () =>
                {
                    _messages.Add(message);
                    await JavaScript.ScrollIntoViewAsync();
                    StateHasChanged();
                });
        
        async Task OnUserTypingAsync(ActorAction actorAction) =>
            await InvokeAsync(() =>
            {
                var (user, isTyping) = actorAction;
                _ = isTyping
                    ? _usersTyping.Add(new Actor(user))
                    : _usersTyping.Remove(new Actor(user));

                Log.LogInformation($"Client receive user typing method: {actorAction.IsTyping}");
                StateHasChanged();
            });

        async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (args is { Key: "Enter" } and { Code: "Enter" })
            {
                await SendMessage();
            }

            if (args is { Key: "ArrowUp" } && _lastCommand is not null)
            {
                _messageInput = _lastCommand.OriginalText;
            }
        }
        
        async Task SendMessage()
        {
            if (_messageInput is { Length: > 0 })
            {
                var channelmessage = new ChannelMessage
                {
                    Id = Guid.NewGuid(),
                    Content = _messageInput,
                    CreatedAt = DateTime.Now,
                    IsEdit = false,
                    Username = CurrentUsername,
                    UserId = CurrentUserId,
                    ChannelId = CurrentChannel.Id
                };
                
                await ChatApi.SaveMessage(channelmessage);
                await _hubConnection.InvokeAsync("PostMessage", channelmessage, CurrentChannel.Id);
                _messageInput = null;
                _messageId = null;

                StateHasChanged();
            }
        }
        
        async Task InitiateDebounceUserIsTyping()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();

            await SetIsTyping(true);
        }
        
        // TODO: Only display in current channel (possible fix? current channel id as parameter in url)
        async Task SetIsTyping(bool isTyping)
        {
            if (_isTyping && isTyping)
            {
                return;
            }

            Log.LogInformation($"Setting is typing: {isTyping}");

            await _hubConnection.InvokeAsync("UserTyping", _isTyping = isTyping);
        }

        async Task AppendToMessage(string text)
        {
            _messageInput += text;
            await SetIsTyping(false);
        }
        
        /*
        async Task StartEdit(ChannelMessage message)
        {
            if (message.Username != CurrentUsername)
            {
                return;
            }
            
            await InvokeAsync(
                async () =>
                {
                    _messageId = message.Id;
                    _messageInput = message.Text;
                    StateHasChanged();
                });
        }   
        */
        async Task DeleteMessageById(ChannelMessage message)
        {
            if (message.Username != CurrentUsername)
            {
                Console.WriteLine("Message not owned by user");
                return;
            }

            await InvokeAsync(async () =>
            {
                await _hubConnection.InvokeAsync("DeleteMessageById", message.Id);
                await ChatApi.DeleteMessageById(message.Id);
            });
        }

        async Task LoadChannelChat(Channel channel)
        {
            await _hubConnection.InvokeAsync("RemoveChannelById", CurrentChannel.Id);
            await _hubConnection.InvokeAsync("JoinChannelById", channel.Id);
            _messages = await ChannelApi.MessagesFomChannelId(channel.Id);
            CurrentChannel = channel;
        }

        public async ValueTask DisposeAsync()
        {
            if (_debounceTimer is { })
            {
                _debounceTimer.Stop();
                _debounceTimer.Dispose();
            }

            if (_hubRegistrations is { Count: > 0 })
            {
                foreach (var disposable in _hubRegistrations)
                {
                    disposable.Dispose();
                }
            }

            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

    }
}