using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dovecord.Shared
{
    // TODO: Populate with user info 
    
    public class User
    {
        public User()
        {
            Users = new Collection<User>();
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public virtual ICollection<User> Users { get; set; }

        //public Color Color { get; set; } Add other info
    }
}