using ModernMediator;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces;

namespace PWMS.Service.Commands
{
	public record CreateMaterialCommand(string sku, string barcode, string name, string description, int categoryId,
		decimal weight, decimal height, decimal width, decimal length) : IRequest<Guid>;

	public class CreateMaterialCommandHandler(
		IMaterialRepository _materialRepository,
		ICategoryRepository _categoryRepository)
		: IValueTaskRequestHandler<CreateMaterialCommand, Guid>
	{
		public async ValueTask<Guid> Handle(CreateMaterialCommand request, CancellationToken ct = default)
		{
			var category = await _categoryRepository.GetByIdAsync(request.categoryId);

			if (category == null)
				throw new Exception("Category not found");

			var ctx = new Material(request.sku, request.barcode, request.name, request.description, category,
				request.weight, request.height, request.width, request.length);

			return ctx.Id;
		}
	}
}
