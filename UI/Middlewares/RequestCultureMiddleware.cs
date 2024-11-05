using System.Globalization;

namespace UI.Middlewares
{
	public class RequestCultureMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly CultureInfo _defaultCulture;

		public RequestCultureMiddleware(RequestDelegate next)
		{
			_next = next;
			_defaultCulture = new CultureInfo("en-US"); // İngiliz kültürü
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Varsayılan kültürü her istek için ayarlayın
			CultureInfo.CurrentCulture = _defaultCulture;
			CultureInfo.CurrentUICulture = _defaultCulture;

			// Sonraki middleware veya işlemi çağırın
			await _next(context);
		}
	}
}
