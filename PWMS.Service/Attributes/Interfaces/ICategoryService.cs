using PWMS.Service.DTO;

namespace PWMS.Service.Attributes.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryDTO>> GetAllAsync();
	}
}
