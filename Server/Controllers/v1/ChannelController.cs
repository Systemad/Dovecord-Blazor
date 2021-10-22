using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dovecord.Data;
using Dovecord.Data.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Dovecord.Server.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/channels")]
    [ApiVersion("1.0")]
    public class ChannelController : ControllerBase
    {
        private readonly ILogger<ChannelController> _logger;
        private IChannelService _channelService;

        static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
        public ChannelController(ILogger<ChannelController> logger, IChannelService channelService)
        {
            _logger = logger;
            _channelService = channelService;
        }   
        
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet(Name = "GetChannels")]
        public async Task<IActionResult> GetChannels()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            return Ok(await _channelService.GetChannelsAsync());
        }
        
        [HttpPost(Name = "CreateChannel")]
        public async Task<IActionResult> CreateChannel([FromRoute]string name)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var channel = new Channel
            {
                Id = Guid.NewGuid(),
                ChannelName = name
            };
            await _channelService.CreateChannelAsync(channel);
            // TODO: Return created channel
            return Ok();
        }
    }
}