using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dovecord.Client;
using Dovecord.Client.Pages.Communication;
using Dovecord.Data;
using Dovecord.Data.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Web.Resource;

namespace Dovecord.Server.Hubs
{
    [Authorize]
    [RequiredScope("API.Access")]
    public class ChatHub : Hub<IChatClient>
    {
        private IUserService _userService;
        public ChatHub(IUserService userService)
        {
            _userService = userService;
        }

        string Username => Context?.User?.Identity?.Name ?? "Unknown";
        Guid UserId => Guid.Parse(Context?.User.FindFirstValue(ClaimTypes.NameIdentifier));


        public override async Task OnConnectedAsync()
        {
            await Clients.Others.UserLoggedOn(new Actor(Username));
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var user = await _userService.GetUserByIdAsync(UserId);
            await _userService.UserLoggedOffAsync(user);
        }

        public async Task PostMessage(ChannelMessage message, Guid channelId)
        {
            await Clients.Group(channelId.ToString()).MessageReceived(message);
        }

        public async Task DeleteMessageById(string messageId)
        {
            await Clients.All.DeleteMessageReceived(messageId);
        }
        public async Task UserTyping(bool isTyping)
            => await Clients.Others.UserTyping(new ActorAction(Username, isTyping));
        
        public async Task JoinChannelById(Guid channelId)
        {
            Console.Write($"Joined channel - {channelId.ToString()}");
            await Groups.AddToGroupAsync(Context.ConnectionId, channelId.ToString());
        }
        
        public async Task RemoveChannelById(Guid channelId)
        {
            Console.Write($"Left channel - {channelId.ToString()}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId.ToString());
        }
    }
}