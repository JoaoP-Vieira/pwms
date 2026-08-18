using PWMS.Core.Entities.Fiscal;

namespace PWMS.Core.Interfaces.Fiscal
{
	public interface IInvoiceRepository
	{
		Task<Invoice?> SelectAsync(string invoiceNumber);
		Task InsertAsync(Invoice invoice);
		Task AssingPlateNumAsync(string invoiceNumber, string plateNum, int? locationId);
		Task UpdateStatusAsync(string invoiceNumber, InvoiceStatus status);
	}
}
