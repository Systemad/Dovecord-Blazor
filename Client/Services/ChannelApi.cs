using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services;

public interface IChannelApi
{
    [Get("/Channel/channels")]
    Task<List<Channel>> GetChannelsAsync();

    [Get("/chat/{id}")]
    Task<List<ChannelMessage>> GetMessagesFomChannelAsync(Guid id);
    
    [Post("/Channel/{channel}")]
    Task<Channel> CreateChannelAsync(string channel);
    
    [Put("/Channel")]
    Task UpdateChannelAsync(Channel channel);
    
    [Delete("/Channel/{id}")]
    Task DeleteChannelAsync(Guid id);
}