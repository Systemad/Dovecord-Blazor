using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data.Services
{
    // TODO: Follow this https://www.youtube.com/watch?v=qEmxoCOH4Uw&t=621s
    public class ChannelService : IChannelService
    {
        private ApplicationDbContext _context;
        
        public ChannelService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Channel>> GetChannelsAsync()
        {
            return await _context.Channels.ToListAsync();
        }
        
        public async Task<bool> DeleteChannelByIdAsync(Guid id)
        {
            var channel = await GetChannelByIdAsync(id);
            _context.Channels.Remove(channel);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> CreateChannelAsync(Channel channel)
        {
            await _context.Channels.AddAsync(channel);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<Channel> GetChannelByIdAsync(Guid id)
        {
            return await _context.Channels.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateChannelAsync(Channel channel)
        {
            var messageToUpdate = await GetChannelByIdAsync(channel.Id);
            messageToUpdate.ChannelName = channel.ChannelName;
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
    }
}