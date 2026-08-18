using Dapper;
using PWMS.Core.Entities.Product;
using PWMS.Core.Interfaces.Product;

namespace PWMS.Infra.Data.Repositories
{
	public class StockItemRepository : BaseRepository, IStockItemRepository
	{
		private const string INSERT_STOCK_ITEM = @"INSERT INTO stock_item
		(
			material_id,
			location_id,
			invoice_id,
			invoice_line_number,
			conference_user_id,
			created_at,
			modified_at
		)
		VALUES
		(
			@MaterialId,
			@LocationId,
			@InvoiceId,
			@InvoiceLineNumber,
			@ConferenceUserId,
			@CreatedAt,
			NULL
		);";

		public StockItemRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task InsertAsync(StockItem stockItem)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(INSERT_STOCK_ITEM,
				new
				{
					MaterialId = stockItem.Material.Id,
					LocationId = stockItem.Location.Id,
					InvoiceId = stockItem.InvoiceId,
					InvoiceLineNumber = stockItem.InvoiceLineNumber,
					ConferenceUserId = stockItem.ConferenceUserId,
					CreatedAt = stockItem.CreatedAt,
				}, transaction);
		}
	}
}
