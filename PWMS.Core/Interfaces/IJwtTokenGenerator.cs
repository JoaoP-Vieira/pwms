using PWMS.Core.Entities;

namespace PWMS.Core.Interfaces
{
	public interface IJwtTokenGenerator
	{
		JwtTokenResult GenerateToken(User user);
	}

	public record JwtTokenResult(string Token, DateTime ExpiresAtUtc);
}
