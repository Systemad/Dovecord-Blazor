using System;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Channel> Channels { get; set; }
    public virtual DbSet<ChannelMessage> ChannelMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guid channelguid = Guid.NewGuid();
            
        modelBuilder.Entity<Channel>().HasData(new Channel
        {
            Id = channelguid,
            Name = "General"
        });
            
        modelBuilder.Entity<Channel>().HasData(new Channel
        {
            Id = Guid.NewGuid(),
            Name = "Random",
        });
            
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = Guid.Parse("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
            Username = "danova",
            Online = false
        });

        modelBuilder.Entity<ChannelMessage>().HasData(new ChannelMessage
        {
            Id = Guid.NewGuid(),
            Content = "First ever channel message",
            CreatedAt = DateTime.Now,
            IsEdit = false,
            Username = "danova",
            UserId = Guid.Parse("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"),
            ChannelId = channelguid
        });
    }
}