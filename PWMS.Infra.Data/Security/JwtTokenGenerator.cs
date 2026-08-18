using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Infra.Data.Security
{
	public sealed class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtSettings _settings;

		public JwtTokenGenerator(IOptions<JwtSettings> options)
		{
			_settings = options.Value;
		}

		public JwtTokenResult GenerateToken(User user)
		{
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
				new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expiresAtUtc = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

			var token = new JwtSecurityToken(
				issuer: _settings.Issuer,
				audience: _settings.Audience,
				claims: claims,
				expires: expiresAtUtc,
				signingCredentials: credentials);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			return new JwtTokenResult(tokenString, expiresAtUtc);
		}
	}
}
