using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Service.DTO;
using PWMS.Service.DTO.Auth;

namespace PWMS.Service.Commands
{
	public record LoginCommand(string email, string password) : IRequest<AuthResultDTO>;

	public class LoginCommandHandler(
		IUserRepository _userRepository,
		IPasswordHasher _passwordHasher,
		IJwtTokenGenerator _jwtTokenGenerator)
		: IValueTaskRequestHandler<LoginCommand, AuthResultDTO>
	{
		public async ValueTask<AuthResultDTO> Handle(LoginCommand request, CancellationToken ct = default)
		{
			var user = await _userRepository.SelectByEmailAsync(request.email);

			if (user == null || !_passwordHasher.Verify(request.password, user.PasswordHash))
				throw new UnauthorizedAccessException("Invalid email or password");

			var tokenResult = _jwtTokenGenerator.GenerateToken(user);

			return new AuthResultDTO
			{
				Token = tokenResult.Token,
				ExpiresAtUtc = tokenResult.ExpiresAtUtc,
				User = new UserDTO
				{
					Id = user.Id,
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName
				}
			};
		}
	}
}
