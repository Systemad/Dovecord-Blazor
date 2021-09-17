using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Dovecord.Client.Extensions;
using Dovecord.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;

namespace Dovecord.Client.Pages.Communication
{
    public partial class Chat : IAsyncDisposable
    {
        readonly Dictionary<string, ActorMessage> _messages = new(StringComparer.OrdinalIgnoreCase);
        readonly HashSet<Actor> _usersTyping = new();
        readonly HashSet<IDisposable> _hubRegistrations = new();
        readonly List<double> _voiceSpeeds =
            Enumerable.Range(0, 12).Select(i => (i + 1) * .25).ToList();
        readonly Timer _debouceTimer = new()
        {
            Interval = 750,
            AutoReset = false
        };
        
        HubConnection _hubConnection;

        string _messageId;
        string _message;
        bool _isTyping;

        ActorCommand _lastCommand;

        [Parameter] public string _messageInput { get; set; }
        //List<SpeechSynthesisVoice> _voices;
        string _voice = "Auto";
        double _voiceSpeed = 1;
        
        public Chat() =>
            _debouceTimer.Elapsed +=
                async (sender, args) => await SetIsTyping(false);

        [Parameter]
        public ClaimsPrincipal User { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public IJSRuntime JavaScript { get; set; }

        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public ILogger<Chat> Log { get; set; }

        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }

        private string isTypingMarkup;
        private string placeholder;
        private string CurrentUsername;
        
        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Nav.ToAbsoluteUri("/chat"),
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

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            CurrentUsername = user.Identity.Name;
            //await UpdateClientVoices(
            //    await JavaScript.GetClientVoices(this));
        }
        
        async ValueTask<string> GetAccessTokenValueAsync()
        {
            var result = await TokenProvider.RequestAccessToken();
            return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
        }

        async Task OnDeleteMessageReceived(string messageId) => await InvokeAsync(async () =>
        {
            _messages.Remove(messageId);
            StateHasChanged();
        });
        
        async Task OnMessageReceivedAsync(ActorMessage message) =>
            await InvokeAsync(
                async () =>
                {
                    _messages[message.Id] = message;
                    //_messages.Add(message.Id, message);
                    Console.WriteLine($" Client - {message.Id} - {message.Text} - {message.User}");
                    /*
                    if (message.IsChatBot && message.SayJoke)
                    {
                        var lang = message.Lang;
                        var voice = _voices?.FirstOrDefault(v => v.Name == _voice);
                        if (voice is not null)
                        {
                            if (!voice.Lang.StartsWith(lang))
                            {
                                var firstLocaleMatchingVoice = _voices.FirstOrDefault(v => v.Lang.StartsWith(lang));
                                if (firstLocaleMatchingVoice is not null)
                                {
                                    lang = firstLocaleMatchingVoice.Lang[0..2];
                                }
                            }
                        }

                        //await JavaScript.SpeakAsync(message.Text, _voice, _voiceSpeed, lang);
                    }

                    await JavaScript.ScrollIntoViewAsync();
                    */
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
        
        bool OwnsMessage(string user) => User.Identity.Name == user;
        
        async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (args is { Key: "Enter" } and { Code: "Enter" })
            {
                await SendMessage();
            }

            if (args is { Key: "ArrowUp" } && _lastCommand is not null)
            {
                _message = _lastCommand.OriginalText;
            }
        }
        
        async Task SendMessage()
        {
            if (_messageInput is { Length: > 0 })
            {
                await _hubConnection.InvokeAsync("PostMessage", _messageInput, _messageId);

                _messageInput = null;
                _messageId = null;

                StateHasChanged();
            }
        }
        
        async Task InitiateDebounceUserIsTyping()
        {
            _debouceTimer.Stop();
            _debouceTimer.Start();

            await SetIsTyping(true);
        }
        
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
            _message += text;
            await SetIsTyping(false);
        }

        async Task StartEdit(ActorMessage message)
        {
            if (message.User != CurrentUsername)
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
        
        async Task DeleteMessageById(ActorMessage message)
        {
            if (message.User != CurrentUsername)
            {
                Console.WriteLine("Message not owned by user");
                return;
            }

            await InvokeAsync(async () =>
            {
                await _hubConnection.InvokeAsync("DeleteMessageById", message.Id);
            });
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_debouceTimer is { })
            {
                _debouceTimer.Stop();
                _debouceTimer.Dispose();
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