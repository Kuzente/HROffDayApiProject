using Microsoft.AspNetCore.Authentication;
using Services.Abstract.UserServices;
using System.Security.Claims;

namespace UI.Middlewares
{
	public class RoleUpdateMiddleware
	{
		private readonly RequestDelegate _next;

		public RoleUpdateMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task Invoke(HttpContext context, IReadUserService _readUserService)
		{
			if (context.User.Identity.IsAuthenticated)
			{
				var userId = context.User.FindFirst(ClaimTypes.NameIdentifier);
				var userRole = context.User.FindFirst(ClaimTypes.Role).Value;
				if (userId is null || userRole is null || !Guid.TryParse(userId.Value, out Guid userIdGuid))
				{
					await context.SignOutAsync();
					context.Response.Redirect("/giris-yap");
					return;
				}
				var user = await _readUserService.GetUserById(userIdGuid);
				if (!user.IsSuccess)
				{
					await context.SignOutAsync();
					context.Response.Redirect("/giris-yap");
					return;
				}
				

				if (!user.Data.Role.ToString().SequenceEqual(userRole))
				{
					// Kullanıcı oturumunu sonlandır
					await context.SignOutAsync();
					// Kullanıcıyı oturumdan çıkardıktan sonra yönlendirme yapılabilir
					context.Response.Redirect("/giris-yap");
					return;
				}


			}

			await _next(context);
		}
	}
}
