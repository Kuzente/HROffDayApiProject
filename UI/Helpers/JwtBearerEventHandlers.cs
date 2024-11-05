using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

namespace UI.Helpers;

public class JwtBearerEventHandlers
{
	private static IConfiguration _configuration;
	public static void Initialize(IConfiguration configuration)
	{
		_configuration = configuration;
	}
	public static JwtBearerEvents CreateEvents() => new JwtBearerEvents
	{
		OnMessageReceived = OnMessageReceived,
		OnChallenge = OnChallenge,
		OnForbidden = OnForbidden,
		OnTokenValidated = OnTokenValidated,
		OnAuthenticationFailed = OnAuthenticationFailed
	};

	public static Task OnMessageReceived(MessageReceivedContext context)
	{
		var token = context.Request.Cookies["jwt"];
		if (!string.IsNullOrEmpty(token))
		{
			context.Token = token;
		}
		else
		{
			context.Fail("Token bulunamadı.");
		}
		return Task.CompletedTask;
	}

	public static Task OnChallenge(JwtBearerChallengeContext context)
	{
		context.HandleResponse();
		context.Response.Redirect("/giris-yap");
		return Task.CompletedTask;
	}

	public static Task OnForbidden(ForbiddenContext context)
	{
		context.Response.Redirect("/403");
		return Task.CompletedTask;
	}

	public static Task OnTokenValidated(TokenValidatedContext context)
	{
		var expClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Exp);
		if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
		{
			var expirationTime = DateTimeOffset.FromUnixTimeSeconds(exp);
			if (expirationTime < DateTimeOffset.UtcNow)
			{
				context.Fail("Token expired");
			}
		}
		return Task.CompletedTask;
	}

	public static Task OnAuthenticationFailed(AuthenticationFailedContext context)
	{
		context.Response.StatusCode = StatusCodes.Status410Gone;
		return Task.CompletedTask;
	}
}
