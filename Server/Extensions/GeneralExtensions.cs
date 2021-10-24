using System.Security.Claims;

namespace Dovecord.Server.Extensions;

public static class GeneralExtensions
{
    public static string GetUserId(this HttpContext httpContext)
    {
        return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}