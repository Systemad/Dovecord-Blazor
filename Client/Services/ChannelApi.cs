using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services
{
    //[Headers("Authorization: Bearer")]
    public interface ChannelApi
    {
        [Get("/Channel/all")]
        Task<List<Channel>> ChannelList();

        [Get("/Channel/{id}")]
        Task<List<ChannelMessage>> MessagesFomChannelId(Guid id);
    }
}