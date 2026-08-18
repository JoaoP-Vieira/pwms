using PWMS.Core.Entities;

namespace PWMS.Core.Interfaces
{
	public interface ICategoryRepository
	{
		Task<IEnumerable<Category>> GetAllAsync();
		Task<Category?> GetByIdAsync(int id);
	}
}
