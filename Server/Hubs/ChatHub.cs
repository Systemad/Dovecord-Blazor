using System;
using System.Security.Claims;
using System.Threading.Tasks;
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

        const string LoginGreetingsFormat =
@"💯 Hi, {0}! This chat application is powered by SignalR 👍🏽 ... Let's command some joke chatbots!
<br>
<br> <strong>Command format:</strong>
<br> &nbsp; <pre><code>(joke|jokes)[:dad|chucknorris][:en (or another two letter locale i.e.; bg)]</code></pre>
<br> <strong>Examples:</strong>
<br> &nbsp; 1) typing ""jokes:chucknorris:bg"" will start the ""Chuck Norris"" chatbot, which will speak jokes continously in Bulgarian.
<br> &nbsp; 2) typing ""joke"" will start the ""Dad"" chatbot, and speak a single joke in English.
<br>
<br> <strong>Notes:</strong>
<br> &nbsp;Anyone can command these and they are shared for all. Type ""stop"" to issue a global stop command. Finally, mix and match single or continous joke(s), joke types and locales...";

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
            await Clients.Caller.MessageReceived(
                new ActorMessage(
                    "greeting", $"Hello, {Username}!", "👋", IsGreeting: true));

            //await Clients.All.SendConnectedUsers(_connections.GetConnections());
            
            await Clients.Others.UserLoggedOn(new Actor(Username));
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            _connections.Remove(Username, Context.ConnectionId);
            await Clients.Others.UserLoggedOff(new Actor(Username));
        }

        public async Task PostMessage(string message, string id = null!)
        {
            /*
            if (_commandSignal.IsRecognizedCommand(Username, message, out var command) &&
                command is not null)
            {
                await Clients.Caller.CommandSignalReceived(command);
                return;
            }
            */
            
            ActorMessage mess = new ActorMessage(UseOrCreateId(id), message, Username, IsEdit: id is not null);
            Console.WriteLine($" Server - {mess.Id} - {mess.Text} - {mess.User}");
            await Clients.All.MessageReceived(mess);
            
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