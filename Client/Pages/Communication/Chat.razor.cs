using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Timers;
using Dovecord.Client.Extensions;
using Dovecord.Client.Pages.Management;
using Dovecord.Client.Services;
using Dovecord.Client.Shared.DTO.Actor;
using Dovecord.Client.Shared.DTO.Channel;
using Dovecord.Client.Shared.DTO.Message;
using Dovecord.Client.Shared.DTO.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dovecord.Client.Pages.Communication;

public partial class Chat : IAsyncDisposable
{
    List<ChannelMessageDto> _messages;
    readonly HashSet<Actor> _usersTyping = new();
    readonly HashSet<IDisposable> _hubRegistrations = new();
    readonly Timer _debounceTimer = new()
    {
        Interval = 750,
        AutoReset = false
    };

    private ChannelDto CurrentChannel { get; set; }
    HubConnection _hubConnection;

    Guid _messageId;
    bool _isTyping;
    private string _createChannel;
    private List<ChannelDto> Channels { get; set; } = new();
    private List<UserDto> Users { get; set; }
    ActorCommand _lastCommand;
    [Parameter] public string _messageInput { get; set; }
    [Parameter] public string CGUID { get; set; }
        
    public Chat() =>
        _debounceTimer.Elapsed +=
            async (sender, args) => await SetIsTyping(false);
        
    [Inject] public Blazored.LocalStorage.ISyncLocalStorageService LocalStorage { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] public IJSRuntime JavaScript { get; set; }
    [Inject] public HttpClient Http { get; set; }
    [Inject] public ILogger<Chat> Log { get; set; }
    [Inject] private IChannelApi ChannelApi { get; set; }
    [Inject] private IMessageApi MessageApi { get; set; }
    [Inject] public IAccessTokenProvider TokenProvider { get; set; }
    
    [Inject] public ISnackbar Snackbar { get; set; }

    private string isTypingMarkup;
    private string _placeholder;
    private string CurrentUsername;
    private Guid _currentUserId;

    protected override async Task OnInitializedAsync()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7045/chathub",
                options => options.AccessTokenProvider =
                    async () => await GetAccessTokenValueAsync())
            //.WithAutomaticReconnect()
            //.AddMessagePackProtocol()
            .Build();

        _hubRegistrations.Add(_hubConnection.OnMessageReceived(OnMessageReceivedAsync));
        _hubRegistrations.Add(_hubConnection.OnUserTyping(OnUserTypingAsync));
        _hubRegistrations.Add(_hubConnection.OnDeleteMessageReceived(OnDeleteMessageReceived));
        _hubRegistrations.Add(_hubConnection.DataRefresh(OnChannelHasChanged));
        await _hubConnection.StartAsync();

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        CurrentUsername = user.Identity?.Name;
        _currentUserId = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
            
        await LoadChannelInfo();
        await LoadChannelChat(CurrentChannel);
    }

    async ValueTask<string> GetAccessTokenValueAsync()
    {
        var result = await TokenProvider.RequestAccessToken();
        return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
    }

    async Task OnDeleteMessageReceived(string messageId) => await InvokeAsync(async () =>
    {
        _messages.Remove(_messages.First(a => a.Id == Guid.Parse(messageId)));
        await JavaScript.ScrollIntoViewAsync();
        StateHasChanged();
    });

    async Task OnMessageReceivedAsync(ChannelMessageDto message) =>
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
                    newmessage.IsEdit = true;
                    newmessage.Content = message.Content;
                    newmessage.CreatedBy = message.CreatedBy;
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
        
    async Task OnUserListReceived(List<UserDto> users) =>
        await InvokeAsync(
            async () =>
            {
                Users = users;
                StateHasChanged();
            });
    
    async Task OnChannelHasChanged() =>
        await InvokeAsync(
            async () =>
            {
                await LoadChannelInfo();
                StateHasChanged();
            });

    async Task SendMessage()
    {
        if (_messageInput is { Length: > 0 })
        {
            var channelmessage = new MessageManipulationDto
            {
                Content = _messageInput,
                ChannelId = CurrentChannel.Id
            };
            /*
            if (_messageId == Guid.Empty)
            {
                channelmessage.Id = Guid.NewGuid();
                await ChatApi.SaveMessage(channelmessage);
            }
            else
            {
                await ChatApi.UpdateMessage(channelmessage);     
            }
            */    
            //await _hubConnection.InvokeAsync("PostMessage", channelmessage, CurrentChannel.Id);
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
        await _hubConnection.InvokeAsync("UserTyping", _isTyping = isTyping);
    }

    async Task AppendToMessage(string text) // for emoji
    {
        _messageInput += text;
        await SetIsTyping(false);
    }
    
    async Task StartEdit(ChannelMessageDto message)
    {
        if (message.CreatedBy != CurrentUsername)
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
        
    async Task DeleteMessageById(ChannelMessageDto message)
    {
        if (message.CreatedBy != CurrentUsername)
        {
            return;
        }

        await InvokeAsync(async () =>
        {
            //await _hubConnection.InvokeAsync("DeleteMessageById", message.Id);
            await MessageApi.DeleteMessageById(message.Id);
        });
    }
    async Task LoadChannelChat(ChannelDto channel)
    {
        await _hubConnection.InvokeAsync("JoinChannel", CurrentChannel.Id);
        await _hubConnection.InvokeAsync("LeaveChannel", channel.Id);
        _messages = await MessageApi.GetMessagesFomChannelAsync(channel.Id);
        CGUID = channel.Id.ToString();
        CurrentChannel = channel;
        LocalStorage.SetItem(_currentUserId.ToString(), CGUID);
        _navigationManager.NavigateTo($"{CGUID}");
    }
        
    async Task LoadChannelInfo()
    {
        Channels = await ChannelApi.GetChannelsAsync();
        var lastChannel = LocalStorage.ContainKey(_currentUserId.ToString());
        if (!lastChannel) 
        {
            CurrentChannel = Channels.First(a => a.Name == "General");
            CGUID = CurrentChannel.Id.ToString();
        }
        else {
            CGUID = LocalStorage.GetItem<string>(_currentUserId.ToString()); 
            CurrentChannel = Channels.First(a => a.Id == Guid.Parse(CGUID));
        }
        _navigationManager.NavigateTo($"{CGUID}");
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
    
    public bool _isOpen;
    public void ToggleOpen()
    {
        _isOpen = !_isOpen;
    }
    // TODO: Send update with Hubs and do StateHasChanged(). then call 
    // Channels = await ChannelApi.GetChannelsAsync(); or LoadChannelInfo() method
    async Task CreateChannelAsync(string channel)
    {
        var parameters = new DialogParameters { ["channel"]=channel };
        var dialog = DialogService.Show<CreateDialog>("Create Channel", parameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var tempchannel = result.Data;
            if ((string)tempchannel is { Length: > 0 } && Channels.Any(c => c.Name != (string)tempchannel))
            {
                var newChannel = await ChannelApi.CreateChannelAsync((ChannelManipulationDto)tempchannel);
                Channels.Add(newChannel);
            }
        }
        _createChannel = string.Empty;
        StateHasChanged();
    }

    private void NavigateChannelSettings()
    {
        _navigationManager.NavigateTo("admin/channels");
    }
}