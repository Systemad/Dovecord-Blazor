using Dovecord.Data.Interfaces;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Dovecord.Server.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/[controller]")]
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
    
    [HttpGet("channels")]
    public async Task<List<Channel>> GetChannels()
    {
        HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
        var channels = await _channelService.GetChannels();
        return channels;
    }
        
    [HttpPost("{name}")]
    public async Task<IActionResult> CreateChannel([FromRoute]string name)
    {
        HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
        var channel = new Channel
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
        await _channelService.CreateChannelAsync(channel);
        return Ok(channel);
    }
}