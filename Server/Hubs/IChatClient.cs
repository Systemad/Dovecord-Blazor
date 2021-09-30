using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;

namespace Dovecord.Server.Hubs
{
    public interface IChatClient
    {
        Task UserLoggedOn(Actor actor);

        Task UserLoggedOff(Actor actor);

        Task UserTyping(ActorAction action);

        Task MessageReceived(ChannelMessage message);

        Task DeleteMessageReceived(string id);

        Task AddToChannel(Guid channelId, Guid userId);
        //Task SendConnectedUsers(IEnumerable<string> users);


        //Task CommandSignalReceived(ActorCommand command);
    }
}
