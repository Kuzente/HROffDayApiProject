using Azure;
using Microsoft.AspNetCore.Authentication;
using Services.Abstract.UserServices;
using System.Security.Claims;

namespace UI.Middlewares
{
	public class RoleUpdateMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;

		public RoleUpdateMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}
		public async Task Invoke(HttpContext context, IReadUserService _readUserService)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);
				var userRole = context.User.FindFirst(ClaimTypes.Role).Value;
				//Cookie de saklanan değerler problemli ise
				if (userId is null || userRole is null || !Guid.TryParse(userId.Value, out Guid userIdGuid))
				{
					context.Response.Cookies.Delete(_configuration["JwtOptions:JwtCookieName"]!);
					context.Response.Redirect("/giris-yap");
					return;
				}
				var user = await _readUserService.GetUserById(userIdGuid);
				//db sorgusu isSuccess ise
				if (!user.IsSuccess ||
					!user.Data.Role.ToString().SequenceEqual(userRole))
				{
					context.Response.Cookies.Delete(_configuration["JwtOptions:JwtCookieName"]!);
					context.Response.Redirect("/giris-yap");
					return;
				}
			}
			
			await _next(context);
		}
	}
}
