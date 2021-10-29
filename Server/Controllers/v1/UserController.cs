using Dovecord.Data;
using Dovecord.Data.Interfaces;
using Dovecord.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dovecord.Server.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(ApplicationDbContext context, IUserService userService)
    {
        _userService = userService;
    }
}