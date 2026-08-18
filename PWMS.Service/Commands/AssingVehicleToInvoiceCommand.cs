using ModernMediator;
using PWMS.Core.Interfaces;
using PWMS.Core.Interfaces.Address;
using PWMS.Core.Interfaces.Fiscal;

namespace PWMS.Service.Commands
{
	public record AssingVehicleToInvoiceCommand(string invoiceNumber, string plateNum) : IRequest<string>;

	public class AssingVehicleToInvoiceCommandHandler(
		IInvoiceRepository _invoiceRepository,
		ILocationRepository _locationRepository,
		IApplicationLogger _logger
	) : IValueTaskRequestHandler<AssingVehicleToInvoiceCommand, string>
	{
		public async ValueTask<string> Handle(AssingVehicleToInvoiceCommand request, CancellationToken cancellationToken = default)
		{
			try
			{
				var avaliableLocation = await _locationRepository.GetAvaliableConferenceLocation();

				if (avaliableLocation == null)
				{
					_logger.LogError($"Can't find a avaliable conference location for: {request.invoiceNumber}");
				}

				await _invoiceRepository.AssingPlateNumAsync(request.invoiceNumber, request.plateNum, avaliableLocation?.Id);

				return avaliableLocation?.Identification ?? "N/D";
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error trying to assing vehicle to invoice: {ex.Message}", ex);

				throw;
			}
		}
	}
}
