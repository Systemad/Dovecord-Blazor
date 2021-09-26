using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Refit;

namespace Dovecord.Client.Services
{
    public interface ChannelApi
    {
        [Get("/Channel/all")]
        //[Headers("Authorization: Bearer")]
        Task<List<Channel>> ChannelList();
    }
}