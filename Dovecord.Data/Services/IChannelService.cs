using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;

namespace Dovecord.Data.Services
{
    public interface IChannelService
    {
        Task<List<Channel>> GetChannelsAsync();
        Task<bool> DeleteChannelByIdAsync(Guid id);
        Task<bool> CreateChannelAsync(Channel channel);
        Task<Channel> GetChannelByIdAsync(Guid id);
        Task<bool> UpdateChannelAsync(Channel channel);
    }
}