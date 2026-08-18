using Dapper;
using PWMS.Core.Interfaces.Fiscal;

namespace PWMS.Infra.Data.Repositories
{
	public class InvoiceItemRepository : BaseRepository, IInvoiceItemRepository
	{
		private const string UPDATE_ITEM_PROCESSED_QUANTITY = @"UPDATE invoice_item
			SET processed_quantity = @ProcessedQuantity
			WHERE invoice_id = @InvoiceId AND line_number = @LineNumber";

		public InvoiceItemRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task UpdateItemAsync(Guid invoiceId, int lineNumber, decimal processedQuantity)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(UPDATE_ITEM_PROCESSED_QUANTITY, new
			{
				ProcessedQuantity = processedQuantity,
				InvoiceId = invoiceId,
				LineNumber = lineNumber,
			}, transaction);
		}
	}
}
