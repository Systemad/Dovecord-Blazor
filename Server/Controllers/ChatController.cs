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
        private ApplicationDbContext _applicationDbContext;

        static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
            _applicationDbContext = new DesignTimeDbContextFactory().CreateDbContext(null!);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task SaveMessageToChannel(ChannelMessage message)
        {
            var channel =
                _applicationDbContext.Channels.Include(a => a.ChannelMessages).First();

            /*
            var newmessage = new ChannelMessage
            {
                Id = Guid.NewGuid(),
                Content = message.Content,
                CreatedAt = DateTime.Now,
                IsEdit = false,
                Username = ,
                UserId = default,
                User = null,
                ChannelId = default,
                Channel = null
            };
            */

            channel.ChannelMessages.Add(message); //.Where(id => id.Id == guid);
            await _applicationDbContext.SaveChangesAsync();
        }
        
        [HttpGet("{channelId:guid}")]
        public List<ChannelMessage> GetMessagesFromChannelid(Guid channelId)
        {
            var messages = _applicationDbContext.ChannelMessages.Where(a => a.ChannelId == channelId).ToList();
            return messages;
        }
        
    }
}