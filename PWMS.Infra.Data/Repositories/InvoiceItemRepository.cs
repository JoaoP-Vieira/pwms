using Dapper;
using PWMS.Core.Entities;
using PWMS.Core.Interfaces.Fiscal;

namespace PWMS.Infra.Data.Repositories
{
	public class InvoiceItemRepository : BaseRepository, IInvoiceItemRepository
	{
		private const string UPDATE_ITEM_PROCESSED_QUANTITY = @"UPDATE invoice_item
			SET processed_quantity = @ProcessedQuantity,
				conference_user_id = @ConferenceUserId,
				conference_date = @ConferenceDate
			WHERE invoice_id = @InvoiceId AND line_number = @LineNumber";

		private const string HAS_UNCONFERED_ITENS = @"SELECT 1
			FROM invoice_item ii
				WHERE ii.invoice_id = @InvoiceId
				AND ii.processed_quantity is null
				AND ii.conference_user_id is null
			LIMIT 1;";

		public InvoiceItemRepository(IPgDbContext dbContext) : base(dbContext) { }

		public async Task UpdateItemAsync(Guid invoiceId, int lineNumber, decimal processedQuantity, int userId)
		{
			var conn = _dbContext.GetConnection();
			var transaction = _dbContext.Transaction;

			await conn.ExecuteAsync(UPDATE_ITEM_PROCESSED_QUANTITY, new
			{
				ProcessedQuantity = processedQuantity,
				InvoiceId = invoiceId,
				LineNumber = lineNumber,
				ConferenceUserId = userId,
				ConferenceDate = DateTime.UtcNow,
			}, transaction);
		}

		public async Task<bool> HasUnconferedItens(Guid invoiceId)
		{
			var conn = _dbContext.GetConnection();

			var result = await conn.QueryFirstOrDefaultAsync(HAS_UNCONFERED_ITENS, new
			{
				InvoiceId = invoiceId,
			});

			return result != null;
		}
	}
}
