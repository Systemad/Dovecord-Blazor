using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dovecord.Data.Services
{
    // TODO: Follow this https://www.youtube.com/watch?v=qEmxoCOH4Uw&t=621s
    public class ChatService : IChatService
    {
        private ApplicationDbContext _context;
        
        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChannelMessage>> GetMessagesByChannelIdAsync(Guid id)
        {
            return await _context.ChannelMessages.Where(a => a.ChannelId == id).ToListAsync();
        }

        public async Task<bool> DeleteMessageByIdAsync(Guid id)
        {
            var message = await GetMessageByIdAsync(id);
            _context.ChannelMessages.Remove(message);
            var deleted = await _context.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> SaveMessageToChannelAsync(ChannelMessage message)
        {

            await _context.ChannelMessages.AddAsync(message);
            var created = await _context.SaveChangesAsync();
            return created > 0;
        }

        public async Task<ChannelMessage> GetMessageByIdAsync(Guid id)
        {
            return await _context.ChannelMessages.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateMessageAsync(ChannelMessage message)
        {
            var messageToUpdate = await GetMessageByIdAsync(message.Id);
            messageToUpdate.Content = message.Content;
            messageToUpdate.IsEdit = true;
            
            //_context.ChannelMessages.Update(messageToUpdate);
            var updated = await _context.SaveChangesAsync();
            return updated > 0;
        }
    }
}