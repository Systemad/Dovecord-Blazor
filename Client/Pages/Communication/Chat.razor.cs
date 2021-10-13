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
    // TODO: fetch all users
    // send as list to UserComponent 
    // https://github.com/MudBlazor/Templates/blob/dev/src/.template.mudblazor/admindashboard/Pages/Applications/Chat/ChatUsers.razor
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
        HubConnection _hubConnection;

        Guid _messageId;
        bool _isTyping;
        private List<Channel> Channels { get; set; } = new List<Channel>();
        private List<User> Users { get; set; }
        ActorCommand _lastCommand;
        [Parameter] public string _messageInput { get; set; }

        public Chat() =>
            _debounceTimer.Elapsed +=
                async (sender, args) => await SetIsTyping(false);
        
        [Inject] public IJSRuntime JavaScript { get; set; }
        [Inject] public HttpClient Http { get; set; }
        [Inject] public ILogger<Chat> Log { get; set; }
        [Inject] private IChannelApi ChannelApi { get; set; }
        [Inject] private IChatApi ChatApi { get; set; }
        [Inject] public IAccessTokenProvider TokenProvider { get; set; }

        private string isTypingMarkup;
        private string placeholder;
        private string CurrentUsername;
        private Guid CurrentUserId;
        [Parameter] public string CGUID { get; set; }
        
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
            _hubRegistrations.Add(_hubConnection.OnUserListReceived(OnUserListReceived));
            await _hubConnection.StartAsync();

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            CurrentUsername = user.Identity?.Name;
            CurrentUserId = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
            Channels = await ChannelApi.ChannelList();
            CurrentChannel = Channels.First(a => a.ChannelName == "General");
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
                    var newmessage = _messages.FirstOrDefault(m => m.Id == message.Id);
                    if (newmessage is null)
                    {
                        _messages.Add(message);
                    }
                    else
                    {
                        newmessage.Content = message.Content;
                        newmessage.CreatedAt = message.CreatedAt;
                    }
                    
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
        
        private async Task OnUserListReceived(List<User> users) =>
            await InvokeAsync(
                async () =>
                {
                    Users = users;
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
                    Id = _messageId,
                    Content = _messageInput,
                    CreatedAt = DateTime.Now,
                    IsEdit = false,
                    Username = CurrentUsername,
                    UserId = CurrentUserId,
                    ChannelId = CurrentChannel.Id
                };

                if (_messageId != Guid.Empty)
                {
                    await ChatApi.UpdateMessage(channelmessage);    
                }
                else
                {
                    await ChatApi.SaveMessage(channelmessage);    
                }
                
                await _hubConnection.InvokeAsync("PostMessage", channelmessage, CurrentChannel.Id);
                _messageInput = null;
                _messageId = Guid.Empty;
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
                    _messageInput = message.Content;
                    StateHasChanged();
                });
        }   
        
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
            CGUID = CurrentChannel.Id.ToString();
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