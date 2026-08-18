using PWMS.Core.Entities;

namespace PWMS.Core.Interfaces
{
	public interface IUserRepository
	{
		Task<int> InsertAsync(User user);
		Task<User?> SelectByEmailAsync(string email);
	}
}
