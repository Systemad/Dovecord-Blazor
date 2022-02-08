using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Client.Shared.DTO.Channel;
using Dovecord.Client.Shared.DTO.Message;
using Refit;

namespace Dovecord.Client.Services;

public interface IChannelApi
{
    [Get("")]
    Task<List<ChannelDto>> GetChannelsAsync();

    [Post("/")]
    Task<ChannelDto> CreateChannelAsync([Body]ChannelManipulationDto channel);
    
    [Put("/{id}")]
    Task UpdateChannelAsync(Guid id, [Body] ChannelManipulationDto channel);
    
    [Delete("/{id}")]
    Task DeleteChannelAsync(Guid id);
}