using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;
using static Dovecord.Data.ModelBuilderExtensions;

namespace Dovecord.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        { } 
        public ApplicationDbContext(DbContextOptions options) : base(options) { } 
        
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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