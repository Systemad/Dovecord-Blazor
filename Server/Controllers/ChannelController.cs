using System.Linq;
using Dovecord.Data;
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

        static readonly string[] scopeRequiredByApi = new string[] { "API.Access" };
     
        public ChannelController(ILogger<ChannelController> logger)
        {
            _logger = logger;
            _applicationDbContext = new DesignTimeDbContextFactory().CreateDbContext(null!);
        }   


        [HttpGet("channel/all")]
        public IActionResult GetChannels()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            
            var channels = _applicationDbContext.Channels.ToList();
            return Ok(channels);
        }

    }
}