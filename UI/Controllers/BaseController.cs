using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class BaseController : Controller
{
    protected string GetClientIpAddress()
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        return ipAddress ?? "IP Address not available";
    }
    protected Guid? GetClientUserId()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
        }
        return null;
    }
}