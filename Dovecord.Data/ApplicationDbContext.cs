using System;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //{ } 
        public ApplicationDbContext(DbContextOptions options) : base(options) { } 
        
        //public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ChannelMessage> ChannelMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guid serverguid = Guid.NewGuid();
            Guid channelguid = Guid.NewGuid();

            /*
            modelBuilder.Entity<Server>().HasData(new Server
            {
                Id = serverguid,
                //Guid = Guid.NewGuid().ToString(),
                ServerName = "Dovecord",
                //Users = null,
                //Channels = null
            });
            */

            modelBuilder.Entity<Channel>().HasData(new Channel
            {
                Id = channelguid,
                //Guid = Guid.NewGuid().ToString(),
                ChannelName = "General",
                //ChannelMessages = null,
                //ServerId = serverguid,
                //Server = null
            });

            
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = Guid.Parse("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
                //Guid = "ca0f4479-5992-4a00-a3d5-d73ae1daff6",
                Username = "danova",
                //ServerId = serverguid
                //Server = null
            });

            modelBuilder.Entity<ChannelMessage>().HasData(new ChannelMessage
            {
                Id = Guid.NewGuid(),
                Content = "First ever channel message",
                CreatedAt = DateTime.Now,
                IsEdit = false,
                Username = "danova",
                UserId = Guid.Parse("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
                //User = null,
                ChannelId = channelguid,
                //Channel = null
            });

            /*
                 modelBuilder.Entity<Server>()
                     .HasMany(u => Channels)
                     .WithMany(cm => cm.Server.);
     
                 modelBuilder.Entity<Server>()
                     .HasMany(u => u.Users);
     
                 modelBuilder.Entity<Server>()
                     .HasMany(c => c.Channels)
                     .WithOne();
     
                 modelBuilder.Entity<Channel>()
                     .HasMany(cm => cm.ChannelMessages)
                     .WithOne();
                     */
        }
    }
}