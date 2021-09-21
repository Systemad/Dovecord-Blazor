using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dovecord.Shared;

namespace Dovecord.Shared
{
    public class Channel
    {
        public Channel()
        {
            ChannelMessages = new Collection<ChannelMessage>();
        }
        
        public int Id { get; set; }
        public string ChannelName { get; set; }
        public virtual ICollection<ChannelMessage> ChannelMessages { get; set; }

        public int ServerId { get; set; }
        public Server Server { get; set; }
    }
    /*
    public class ChannelMessage
    {
        public string MessageId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdit { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
    */
}