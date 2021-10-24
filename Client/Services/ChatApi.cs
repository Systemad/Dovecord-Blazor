using System;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services;

public interface IChatApi
{
    [Delete("/Chat/delete/{id}")]
    Task DeleteMessageById(Guid id);
        
    [Post("/Chat/save")]
    Task SaveMessage([Body]ChannelMessage message);
        
    [Put("/Chat/update")]
    Task UpdateMessage([Body]ChannelMessage message);
}