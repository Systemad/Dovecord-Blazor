using Dovecord.Shared;

namespace Dovecord.Data.Interfaces;

public interface IChatService
{
    Task<List<ChannelMessage>> GetMessagesByChannelIdAsync(Guid id);
    Task<bool> DeleteMessageByIdAsync(Guid id);
    Task<bool> SaveMessageToChannelAsync(ChannelMessage id);
    Task<ChannelMessage> GetMessageByIdAsync(Guid id);
    Task<bool> UpdateMessageAsync(ChannelMessage message);
    Task<bool> UserOwnsMessageAsync(Guid postId, string userId);
}