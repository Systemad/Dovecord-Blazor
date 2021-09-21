using System;

namespace Dovecord.Shared
{
    public class ChannelMessage
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdit { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}