using System;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services;

public interface IChatApi
{
    [Delete("/chat/{id}")]
    Task DeleteMessageById(Guid id);
        
    [Post("/chat")]
    Task SaveMessage([Body]ChannelMessage message);
        
    [Put("/chat")]
    Task UpdateMessage([Body]ChannelMessage message);
}