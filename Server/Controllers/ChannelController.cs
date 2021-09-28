using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Data;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Dovecord.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChannelController : ControllerBase
    {
        private readonly ILogger<ChannelController> _logger;
        private ApplicationDbContext _applicationDbContext;

        static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
        public ChannelController(ILogger<ChannelController> logger)
        {
            _logger = logger;
            _applicationDbContext = new DesignTimeDbContextFactory().CreateDbContext(null!);
        }   

        
        //[AllowAnonymous]
        [HttpGet("all")]
        public List<Channel> GetChannels()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            
            var channels = _applicationDbContext.Channels.ToList();
            return channels;
        }

        //[AllowAnonymous]
        [HttpGet("{channelId:guid}")]
        public List<ChannelMessage> GetMessagesFromChannelid(Guid channelId)
        {
            var messages = _applicationDbContext.ChannelMessages.Where(a => a.ChannelId == channelId).ToList();
            return messages;
        }

    }
}