using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Client.Shared.DTO.Message;
using Refit;

namespace Dovecord.Client.Services;

public interface IMessageApi
{
    [Delete("/{id}")]
    Task DeleteMessageById(Guid id);
        
    [Post("")]
    Task SaveMessage([Body]MessageManipulationDto message);
        
    [Put("/{id}")]
    Task UpdateMessage([Body]MessageManipulationDto message);
    
    [Get("/channel/{id}")]
    Task<List<ChannelMessageDto>> GetMessagesFomChannelAsync(Guid id);
}