using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Data.Interfaces;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data.Services;

public class ChannelService : IChannelService
{
    private ApplicationDbContext _context;
        
    public ChannelService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChannelMessage>> GetMessagesByChannelIdAsync(Guid id)
    {
        return await _context.ChannelMessages.Where(a => a.ChannelId == id).ToListAsync();
    }

    public async Task<List<Channel>> GetChannels()
    {
        return await _context.Channels.ToListAsync();
    }

    public async Task<bool> DeleteChannelAsync(Guid id)
    {
        var channel = await GetChannelByIdAsync(id);
        _context.Channels.Remove(channel);
        var deleted = await _context.SaveChangesAsync();
        return deleted > 0;
    }

    public async Task<bool> CreateChannelAsync(Channel channel)
    {
        var count = await _context.Channels.CountAsync();
        
        if(count < 10)
            await _context.Channels.AddAsync(channel);
        
        // Add error code
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<Channel> GetChannelByIdAsync(Guid id)
    {
        return await _context.Channels.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> UpdateChannelAsync(Channel channel)
    {
        var channelToUpdate = await GetChannelByIdAsync(channel.Id);
        channelToUpdate.ChannelName = channel.ChannelName;
        var updated = await _context.SaveChangesAsync();
        return updated > 0;
    }
}