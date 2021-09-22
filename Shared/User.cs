using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Dovecord.Shared
{
    
    public class User
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Username { get; set; }
        //public Color Color { get; set; }
        
        public int ServerId { get; set; }
        public Server Server { get; set; }
        //public Dovecord Dovecord { get; set; }
    }
}