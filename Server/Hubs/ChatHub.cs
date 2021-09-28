using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        //readonly ICommandSignalService _commandSignal;
        
        string Username => Context?.User?.Identity?.Name ?? "Unknown";
        private string UserId => Context?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        //public ChatHub(ICommandSignalService commandSignal) => _commandSignal = commandSignal;

        public override async Task OnConnectedAsync()
        {
            
            Console.WriteLine(Username);
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
            /*
            if (_commandSignal.IsRecognizedCommand(Username, message, out var command) &&
                command is not null)
            {
                await Clients.Caller.CommandSignalReceived(command);
                return;
            }
            */

            //ActorMessage mess = new ActorMessage(UseOrCreateId(id), message, Username, IsEdit: id is not null);
            //Console.WriteLine($" Server - {mess.Id} - {mess.Text} - {mess.User}");

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
            
            
            await Clients.All.MessageReceived(channelmessage);
            
            //await Clients.All.MessageReceived(
                //new ActorMessage(UseOrCreateId(id), message, Username, IsEdit: id is not null));
        }

        public async Task DeleteMessageById(string messageId)
        {
            await Clients.All.DeleteMessageReceived(messageId);
        }
        public async Task UserTyping(bool isTyping)
            => await Clients.Others.UserTyping(new ActorAction(Username, isTyping));

        static string UseOrCreateId(string id)
            => string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id;
    }
}