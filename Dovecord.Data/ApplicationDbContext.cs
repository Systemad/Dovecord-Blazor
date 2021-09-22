using System;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;
using static Dovecord.Data.ModelBuilderExtensions;

namespace Dovecord.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //{ } 
        public ApplicationDbContext(DbContextOptions options) : base(options) { } 
        
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        
        public virtual DbSet<ChannelMessage> ChannelMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Server>().HasData(new Server
            {
                Id = 1,
                Guid = Guid.NewGuid().ToString(),
                ServerName = "Dovecord",
                //Users = null,
                //Channels = null
            });

            modelBuilder.Entity<Channel>().HasData(new Channel
            {
                Id = 1,
                Guid = Guid.NewGuid().ToString(),
                ChannelName = "General",
                //ChannelMessages = null,
                ServerId = 1,
                //Server = null
            });

            
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Guid = "ca0f4479-5992-4a00-a3d5-d73ae1daff6",
                Username = "danova",
                ServerId = 1,
                //Server = null
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