using PWMS.Core.Entities.Product;

namespace PWMS.Core.Interfaces.Product
{
	public interface IStockItemRepository
	{
		Task InsertAsync(StockItem stockItem);
	}
}
