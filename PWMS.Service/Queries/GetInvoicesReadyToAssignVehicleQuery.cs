using ModernMediator;
using PWMS.Core.Interfaces.Fiscal;
using PWMS.Service.DTO.Fiscal;

namespace PWMS.Service.Queries
{
	public record GetInvoicesReadyToAssignVehicleQuery : IRequest<IEnumerable<InvoiceReadyToAssignDTO>>;

	public class GetInvoicesReadyToAssignVehicleQueryHandler : IValueTaskRequestHandler<GetInvoicesReadyToAssignVehicleQuery, IEnumerable<InvoiceReadyToAssignDTO>>
	{
		private readonly IInvoiceRepository _invoiceRepository;

		public GetInvoicesReadyToAssignVehicleQueryHandler(IInvoiceRepository invoiceRepository)
		{
			_invoiceRepository = invoiceRepository;
		}

		public async ValueTask<IEnumerable<InvoiceReadyToAssignDTO>> Handle(GetInvoicesReadyToAssignVehicleQuery request, CancellationToken cancellationToken = default)
		{
			var result = await _invoiceRepository.SelectInvoicesReadyToAssignVehicleAsync();

			return result.Select(x => new InvoiceReadyToAssignDTO()
			{
				InvoiceNumber = x.InvoiceNumber,
				Series = x.Series,
				IssueDate = x.IssueDate,
				TotalAmount = (int)x.TotalAmount,
				TotalVolumes = (int)x.TotalVolumes,
				TotalItens = (int)x.TotalItens,
				CreatedAt = x.CreatedAt,
				Issuer = x.Issuer ?? string.Empty,
				Recipient = x.Recipient ?? string.Empty,
				Carrier = x.Carrier ?? string.Empty,
			});
		}
	}
}
