using ModernMediator;
using PWMS.Core.Entities.Fiscal;
using PWMS.Core.Interfaces;
using PWMS.Core.Interfaces.Fiscal;
using PWMS.Service.RabbitMQ.Commands;

namespace PWMS.Service.Commands
{
	public record ConferInvoiceItemBody(string barCode, decimal quantity);
	public record ConferInvoiceItemCommand(string barCode, decimal quantity, int userId) : IRequest<bool>;

	public class ConferInvoiceItemCommandHandler(
		IUnitOfWork _unitOfWork,
		IInvoiceRepository _invoiceRepository,
		IInvoiceItemRepository _invoiceItemRepository,
		IApplicationLogger _logger,
		IMediator _mediator
	) : IValueTaskRequestHandler<ConferInvoiceItemCommand, bool>
	{
		public async ValueTask<bool> Handle(ConferInvoiceItemCommand request, CancellationToken cancellationToken = default)
		{
			if (request.quantity < 0)
			{
				_logger.LogError($"Invalid quantity");
				throw new ArgumentException($"Invalid quantity", "quantity");
			}

			if (string.IsNullOrWhiteSpace(request.barCode) || request.barCode.Length < 11)
			{
				_logger.LogError($"{request.barCode} isn't a valid invoice item code");
				throw new ArgumentException($"{request.barCode} isn't a valid invoice item code", "barCode");
			}

			var invoiceNumber = request.barCode.Substring(0, request.barCode.Length - 2);

			if (!int.TryParse(request.barCode.Substring(request.barCode.Length - 2), out int lineNumber))
			{
				_logger.LogError($"{request.barCode} isn't a valid invoice item code");
				throw new ArgumentException($"{request.barCode} isn't a valid invoice item code", "barCode");
			}

			var invoice = await _invoiceRepository.SelectAsync(invoiceNumber);

			if (invoice == null)
			{
				_logger.LogError($"Invoice {invoiceNumber} not found");
				throw new InvalidOperationException($"Invoice {invoiceNumber} not found");
			}

			try
			{
				await _unitOfWork.BeginTransactionAsync();

				invoice.CheckStartConference();

				if (invoice.Status == InvoiceStatus.Pending)
					await _invoiceRepository.UpdateStatusAsync(invoice.InvoiceNumber, InvoiceStatus.InConference);

				await _invoiceItemRepository.UpdateItemAsync(invoice.Id, lineNumber, request.quantity, request.userId);

				if (!await _invoiceItemRepository.HasUnconferedItens(invoice.Id))
				{
					await _mediator.SendAsync(new PublishConferenciaNFCommand(
						invoice.InvoiceNumber,
						invoice.Id,
						request.userId
					), cancellationToken);
				}

				await _unitOfWork.CommitAsync();

				return true;
			}
			catch (Exception ex)
			{
				await _unitOfWork.RollbackAsync();

				_logger.LogError($"Error creating invoice: {ex.Message}", ex);

				throw;
			}
		}
	}
}
