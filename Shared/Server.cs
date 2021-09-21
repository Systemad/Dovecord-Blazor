using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dovecord.Shared
{
    public class Server
    {
        public Server()
        {
            Channels = new Collection<Channel>();
            Users = new Collection<User>();
        }
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ServerName { get; set; }
        
        //[ForeignKey("User")]
        public virtual ICollection<User> Users { get; set; }
        
        //[ForeignKey("User")]
        public virtual ICollection<Channel> Channels { get; set; }
    }
}