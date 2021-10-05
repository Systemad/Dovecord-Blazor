using System;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace Dovecord.Client.Shared
{
    public partial class MainBody
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        
        //[Inject] private IUserApi UserApi { get; set; }
        
        private string CurrentUsername;
        private Guid CurrentUserId;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            CurrentUsername = user.Identity?.Name;
            CurrentUserId = Guid.Parse(authState.User.Claims.FirstOrDefault(c => c.Type == "sub").Value);
            
            //UserApi
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
    
}