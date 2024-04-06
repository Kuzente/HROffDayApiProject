using System.Security.Claims;
using Core.Enums;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;


namespace Services.HangfireFilter
{
	public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpContext = context.GetHttpContext();
			if (httpContext.User.Identity.IsAuthenticated && httpContext.User.FindFirst(ClaimTypes.Role)?.Value == nameof(UserRoleEnum.HumanResources))
			{
				return true;
			}

			return false;

		}
	}
}
