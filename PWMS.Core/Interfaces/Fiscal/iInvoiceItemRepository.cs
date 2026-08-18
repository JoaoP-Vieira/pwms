namespace PWMS.Core.Interfaces.Fiscal
{
	public interface IInvoiceItemRepository
	{
		Task UpdateItemAsync(Guid invoiceId, int lineNumber, decimal processedQuantity, int userId);
	}
}
