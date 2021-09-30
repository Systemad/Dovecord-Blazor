using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Dovecord.Client.Pages.Communication;
using Dovecord.Data;
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
        
        private static readonly ConnectionMapping<string> _connections = 
            new ConnectionMapping<string>();
        
        private ApplicationDbContext _context;

        public ChatHub()
        {
            _context = new DesignTimeDbContextFactory().CreateDbContext(null!);
        }
        //readonly ICommandSignalService _commandSignal;
        
        string Username => Context?.User?.Identity?.Name ?? "Unknown";
        private string UserId => Context?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //public ChatHub(ICommandSignalService commandSignal) => _commandSignal = commandSignal;

        public override async Task OnConnectedAsync()
        {
            
            Console.WriteLine($"{UserId} -  {Context.ConnectionId}");
            _connections.Add(Username, Context.ConnectionId);
            /*
            await Clients.Caller.MessageReceived(
                new ActorMessage(
                    "greeting", string.Format(LoginGreetingsFormat, Username), "👋", IsGreeting: true));
            */

            /*
            await Clients.Caller.MessageReceived(
                new ActorMessage(
                    "greeting", $"Hello, {Username}!", "👋", IsGreeting: true));
                    */

            //await Clients.All.SendConnectedUsers(_connections.GetConnections());
            
            await Clients.Others.UserLoggedOn(new Actor(Username));
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            _connections.Remove(Username, Context.ConnectionId);
            await Clients.Others.UserLoggedOff(new Actor(Username));
        }

        public async Task PostMessage(string message, Guid channelId)
        {
            var channelmessage = new ChannelMessage
            {
                Id = Guid.NewGuid(),
                Content = message,
                CreatedAt = DateTime.Now,
                IsEdit = false,
                Username = Username,
                UserId = Guid.Parse(UserId),
                ChannelId = channelId,
            };
            await Clients.Group(channelId.ToString()).MessageReceived(channelmessage);
            _context.ChannelMessages.Add(channelmessage);
            await _context.SaveChangesAsync();
            //await Clients.All.MessageReceived(
                //new ActorMessage(UseOrCreateId(id), message, Username, IsEdit: id is not null));
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