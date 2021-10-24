using Dovecord.Data;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Dovecord.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChannelController : ControllerBase
{
    private readonly ILogger<ChannelController> _logger;
    private ApplicationDbContext _applicationDbContext;

    static readonly string[] scopeRequiredByApi = new[] { "API.Access" };
     
    public ChannelController(ILogger<ChannelController> logger, ApplicationDbContext applicationDbContext)
    {
        _logger = logger;
        _applicationDbContext = applicationDbContext;
    }   

        
    //[AllowAnonymous]
    [HttpGet("all")]
    public List<Channel> GetChannels()
    {
        HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            
        var channels = _applicationDbContext.Channels.ToList();
        return channels;
    }
        
    [HttpPut("create/{name}")]
    public IActionResult CreateChannel([FromRoute]string name)
    {
        HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
        var channel = new Channel
        {
            Id = Guid.NewGuid(),
            ChannelName = name,
        };
        _applicationDbContext.Channels.Add(channel);
        _applicationDbContext.SaveChangesAsync();
        return Ok($"Channel {name} created");
    }
}