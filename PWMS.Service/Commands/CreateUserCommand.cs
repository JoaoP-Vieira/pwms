using ModernMediator;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;
using PWMS.Service.DTO;

namespace PWMS.Service.Commands
{
	public record CreateUserCommand(string email, string firstName, string lastName, string password) : IRequest<UserDTO>;

	public class CreateUserCommandHandler(
		IUserRepository _userRepository,
		IPasswordHasher _passwordHasher)
		: IValueTaskRequestHandler<CreateUserCommand, UserDTO>
	{
		public async ValueTask<UserDTO> Handle(CreateUserCommand request, CancellationToken ct = default)
		{
			var existingUser = await _userRepository.SelectByEmailAsync(request.email);

			if (existingUser != null)
				throw new ArgumentException("E-mail already registered", nameof(request.email));

			var passwordHash = _passwordHasher.Hash(request.password);

			var user = new User(request.email, request.firstName, request.lastName, passwordHash);

			var id = await _userRepository.InsertAsync(user);

			return new UserDTO
			{
				Id = id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}
	}
}
