using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dovecord.Shared
{
    public class Server
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServerId { get; set; }
        
        public string ServerName { get; set; }
        
        //[ForeignKey("User")]
        public virtual ICollection<User> Users { get; set; }
        
        //[ForeignKey("User")]
        public virtual ICollection<Channel> Channels { get; set; }
    }
}