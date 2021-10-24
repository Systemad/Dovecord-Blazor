using System;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Client.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dovecord.Client.Shared;

public partial class MainBody
{
    [Parameter] public RenderFragment ChildContent { get; set; }
        
    [Inject] private IUserApi UserApi { get; set; }
        
    private User CurrentUser;
    private string CurrentUsername;
    private Guid CurrentUserId;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        CurrentUser = new User
        {
            Id = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value),
            Username = user.Identity?.Name
        };
        //CurrentUsername = user.Identity?.Name;
        //CurrentUserId = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
        //await UserApi.SendConnectedUser(CurrentUser);
        //await hubConnection.StartAsync();
    }

    private async Task BeginSignOut(MouseEventArgs args)
    {
        await SignOutManager.SetSignOutState();
        _navigationManager.NavigateTo("authentication/logout");
    }
        
        
    /*
    private HubConnection hubConnection;
    public bool IsConnected => hubConnection.State == HubConnectionState.Connected;
    */
}