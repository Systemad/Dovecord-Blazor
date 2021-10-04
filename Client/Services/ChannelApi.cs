using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services
{
    //[Headers("Authorization: Bearer")]
    public interface IChannelApi
    {
        [Get("/Channel/all")]
        Task<List<Channel>> ChannelList();

        [Get("/Chat/{id}")]
        Task<List<ChannelMessage>> MessagesFomChannelId(Guid id);
    }
}