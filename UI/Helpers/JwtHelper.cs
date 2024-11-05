using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UI.Helpers
{
	public class JwtHelper
	{
		private readonly IConfiguration _configuration;

		public JwtHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateJwtToken(List<Claim> claims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_new_very_long_secret_key_here_which_is_at_least_32_characters_long"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtOptions:Issuer"],
				audience: _configuration["JwtOptions:Audience"],
				claims: claims,
				notBefore: DateTime.Now,
				expires: DateTime.Now.AddHours(Convert.ToDouble(_configuration["JwtOptions:ExpirePerHour"])),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
