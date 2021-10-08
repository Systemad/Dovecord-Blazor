using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Dovecord.Server.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}