using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;

namespace Dovecord.Data.Interfaces;

public interface IChannelService
{
    Task<List<Channel>> GetChannels();
    Task<bool> DeleteChannelAsync(Guid id);
    Task<bool> CreateChannelAsync(Channel id);
    Task<Channel> GetChannelByIdAsync(Guid id);
    Task<bool> UpdateChannelAsync(Channel message);
}