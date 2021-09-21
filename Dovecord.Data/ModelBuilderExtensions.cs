using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data
{
    public class ModelBuilderExtensions
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Server>().HasData(new Server
            {
                Id = 0,
                ServerName = "Dovecord",
                //Users = null,
                //Channels = null
            });

            modelBuilder.Entity<Channel>().HasData(new Channel
            {
                Id = 0,
                ChannelName = "General",
                //ChannelMessages = null,
                ServerId = 0,
                //Server = null
            });

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = "ca0f4479-5992-4a00-a3d5-d73ae1daff6",
                Username = "danova",
                ServerId = 0,
                //Server = null
            });
        }
        
    }
}