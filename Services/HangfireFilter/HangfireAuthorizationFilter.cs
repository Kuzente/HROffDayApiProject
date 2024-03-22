using Hangfire.Annotations;
using Hangfire.Dashboard;


namespace Services.HangfireFilter
{
	public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize([NotNull] DashboardContext context)
		{
			var httpContext = context.GetHttpContext();
			return httpContext.User.Identity?.IsAuthenticated ?? false;
		}
	}
}
