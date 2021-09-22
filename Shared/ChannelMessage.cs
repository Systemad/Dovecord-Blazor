using System;

namespace Dovecord.Shared
{
    public class ChannelMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdit { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        
        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}