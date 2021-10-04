using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Data;
using Dovecord.Data.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private IChatService _chatService;

        static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
        public ChatController(ILogger<ChatController> logger, ApplicationDbContext context, IChatService chatService)
        {
            _logger = logger;
            _context = context;
            _chatService = chatService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveMessageToChannel([FromBody] ChannelMessage message)
        {
            if (message is null)
                return BadRequest("Message is null");
            
            await _chatService.SaveMessageToChannelAsync(message);
            return Ok();
        }
        
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMessage([FromBody] ChannelMessage message)
        {
            if (message is null)
                return BadRequest("Message is null");
            
            await _chatService.UpdateMessageAsync(message);
            return Ok();
        }
        
        [HttpGet("{channelId:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Channel))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessagesFromChannelId(Guid channelId)
        {
            return Ok(await _chatService.GetMessagesByChannelIdAsync(channelId));
        }
        
        [HttpDelete("delete/{messageId:guid}")]
        public async Task<IActionResult> DeleteMessageById(Guid messageId)
        {
            var messagedeleted = await _chatService.DeleteMessageByIdAsync(messageId);
            return messagedeleted ? NoContent() : NotFound();
        }
        
    }
}