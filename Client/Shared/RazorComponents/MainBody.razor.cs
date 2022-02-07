using System;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Client.Services;
using Dovecord.Client.Shared.DTO.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Dovecord.Client.Shared.RazorComponents;

public partial class MainBody
{
    [Parameter] public RenderFragment ChildContent { get; set; }
        
    [Inject] private IUserApi UserApi { get; set; }
        
    private UserDto CurrentUser;
    private string CurrentUsername;
    private Guid CurrentUserId;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        /*
        CurrentUser = new User
        {
            Id = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value),
            Username = user.Identity?.Name
        };
        */
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