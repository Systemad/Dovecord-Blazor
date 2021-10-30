using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dovecord.Shared
{
    public class Channel
    {
        public Channel()
        {
            ChannelMessages = new Collection<ChannelMessage>();
        }
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ChannelMessage> ChannelMessages { get; set; }
    }
}