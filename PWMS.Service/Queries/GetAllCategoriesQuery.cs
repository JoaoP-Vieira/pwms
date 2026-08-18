using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Service.DTO;

namespace PWMS.Service.Queries
{
	public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDTO>>;

	public class GetAllCategoriesQueryHandler : IValueTaskRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
	{
		private readonly ICategoryRepository _categoryRepository;

		public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async ValueTask<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken = default)
		{
			var result = await _categoryRepository.GetAllAsync();

			return result.Select(x => new CategoryDTO()
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
			});
		}

	}
}
