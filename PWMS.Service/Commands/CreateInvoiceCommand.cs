using ModernMediator;
using PWMS.Core.Entities;
using PWMS.Core.Entities.Fiscal;
using PWMS.Core.Interfaces;
using PWMS.Core.Interfaces.Fiscal;

namespace PWMS.Service.Commands
{
	public record CreateInvoiceItemCommand(int lineNumber, decimal declaredQuantity, decimal unityPrice, string materialSku);

	public record CreateInvoiceCommand(string invoiceNumber, string series, string verificationCode, int type,
			string issuerDocument, string issuerName, string issuerAddress,
			string recipientDocument, string recipientName, string recipientAddress,
			string? carrierDocument, string? carrierName, string? carrierAddress,
			DateTime issueDate, DateTime? expectedDeliveryDate, decimal totalAmount,
			int totalVolumes, decimal totalGrossWeight, IEnumerable<CreateInvoiceItemCommand> items) : IRequest<Guid>;


	public class CreateInvoiceCommandHandler(
		IUnitOfWork _unitOfWork,
		IPersonRepository _personRepository,
		IMaterialRepository _materialRepository,
		IInvoiceRepository _invoiceRepository,
		IApplicationLogger _logger)
		: IValueTaskRequestHandler<CreateInvoiceCommand, Guid>
	{
		public async ValueTask<Guid> Handle(CreateInvoiceCommand request, CancellationToken ct = default)
		{
			_logger.LogInformation("Starting invoice creation with invoice number: {InvoiceNumber}", request.invoiceNumber);

			List<InvoiceItem> ctxInvoiceItems = new List<InvoiceItem>();

			foreach (var item in request.items)
			{
				var material = await _materialRepository.GetBySkuAsync(item.materialSku);

				if (material == null)
				{
					_logger.LogError($"Material {item.materialSku} not registered in the database");
					throw new Exception($"Material {item.materialSku} not registered in the database");
				}

				ctxInvoiceItems.Add(new InvoiceItem(item.lineNumber, item.declaredQuantity, item.unityPrice, material));
			}

			_logger.LogInformation("Invoice items validated. Total items: {ItemCount}", ctxInvoiceItems.Count);

			await _unitOfWork.BeginTransactionAsync();

			try
			{
				var issuer = await GetOrCreatePerson(request.issuerName, request.issuerDocument, request.issuerAddress);
				var recipient = await GetOrCreatePerson(request.recipientName, request.recipientDocument, request.recipientAddress);
				Person? carrier = null;

				if (!string.IsNullOrWhiteSpace(request.carrierName) && !string.IsNullOrWhiteSpace(request.carrierDocument) && !string.IsNullOrWhiteSpace(request.carrierAddress))
				{
					carrier = await GetOrCreatePerson(request.carrierName, request.carrierDocument, request.carrierAddress);
				}

				var ctxInvoice = new Invoice(request.invoiceNumber, request.series, request.verificationCode, request.type,
					issuer, recipient, carrier, request.issueDate, request.expectedDeliveryDate, request.totalAmount, request.totalVolumes,
					request.totalVolumes, ctxInvoiceItems.ToList(), DateTime.UtcNow, null);

				await _invoiceRepository.InsertAsync(ctxInvoice);

				await _unitOfWork.CommitAsync();

				_logger.LogInformation("Invoice successfully created with ID: {InvoiceId}", ctxInvoice.Id);

				return ctxInvoice.Id;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackAsync();

				_logger.LogError($"Error creating invoice: {ex.Message}", ex);

				throw;
			}
		}

		private async Task<Person> GetOrCreatePerson(string name, string document, string address)
		{
			var person = await _personRepository.SelectByDocument(document);

			if (person != null)
				return person;

			var ctx = new Person(name, document, address);

			ctx.SetId(await _personRepository.InsertAsync(ctx));

			return ctx;
		}
	}
}
