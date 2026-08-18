using PWMS.Core.Interfaces;
using PWMS.Service.Attributes.Interfaces;
using PWMS.Service.DTO;

namespace PWMS.Service.Attributes
{
	public class CategoryService : ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryService(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
		{
			var data = await _categoryRepository.GetAllAsync();
			
			var teste = await _categoryRepository.GetAllAsync();

			return data.Select(x => new CategoryDTO {Id = x.Id, Name = x.Name, Description = x.Description});
		}
	}
}
