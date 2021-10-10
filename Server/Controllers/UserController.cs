using System.Threading.Tasks;
using Dovecord.Data;
using Dovecord.Data.Services;
using Dovecord.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dovecord.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;
        private IUserService _userService;

        public UserController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> UserConnected([FromBody] User user)
        {
            var exist = await _userService.CheckIfUserExistAsync(user.Id);

            if (!exist)
                await _userService.CreateUserAsync(user);

            await _userService.UserLoggedOnAsync(user);
            
            return NoContent();
        }
        
    }
}