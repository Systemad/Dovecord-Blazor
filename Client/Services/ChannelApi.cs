using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services;

public interface IChannelApi
{
    [Get("/Channel/channels")]
    Task<List<Channel>> ChannelList();

    [Get("/chat/{id}")]
    Task<List<ChannelMessage>> MessagesFomChannelId(Guid id);
    
    [Post("/Channel/{channel}")]
    Task CreateChannel(string channel);
}