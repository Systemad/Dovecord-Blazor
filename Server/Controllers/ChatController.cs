using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Data;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dovecord.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private ApplicationDbContext _context;

        static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
            _context = new DesignTimeDbContextFactory().CreateDbContext(null!);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task SaveMessageToChannel(ChannelMessage message)
        {
            var channel =
                _context.Channels.Include(a => a.ChannelMessages).First();
            channel.ChannelMessages.Add(message); //.Where(id => id.Id == guid);
            await _context.SaveChangesAsync();
        }
        
        [HttpGet("{channelId:guid}")]
        public List<ChannelMessage> GetMessagesFromChannelid(Guid channelId)
        {
            var messages = _context.ChannelMessages.Where(a => a.ChannelId == channelId).ToList();
            return messages;
        }
        
        [HttpDelete("delete/{messageId:guid}")]
        public async Task DeleteMessageBy(Guid messageId)
        {
            var message = _context.ChannelMessages.FindAsync(messageId);
            _context.ChannelMessages.Remove(await message);
            await _context.SaveChangesAsync();
        }
        
    }
}