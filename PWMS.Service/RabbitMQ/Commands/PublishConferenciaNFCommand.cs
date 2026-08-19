using ModernMediator;
using PWMS.Service.RabbitMQ.Interfaces;
using PWMS.Service.RabbitMQ.Models;
using PWMS.Core.Interfaces;

namespace PWMS.Service.RabbitMQ.Commands
{
	public record PublishConferenciaNFCommand(
		string InvoiceNumber,
		Guid InvoiceId,
		int UserId
	) : IRequest<bool>;

	public class PublishConferenciaNFCommandHandler(
		IRabbitMQPublisher _publisher,
		IApplicationLogger _logger
	) : IValueTaskRequestHandler<PublishConferenciaNFCommand, bool>
	{
		private const string QueueName = "queue.conferencia-nf.v1";
		private const string ExchangeName = "exchange.conferencia-nf.v1";
		private const string RoutingKeyName = "routing.conferencia-nf.v1";

		public async ValueTask<bool> Handle(PublishConferenciaNFCommand request, CancellationToken cancellationToken = default)
		{
			try
			{
				var message = new ConferenciaNFMessage
				{
					InvoiceNumber = request.InvoiceNumber,
					InvoiceId = request.InvoiceId,
					UserId = request.UserId,
					ConferredAt = DateTime.UtcNow
				};

				await _publisher.PublishAsync(QueueName, ExchangeName, RoutingKeyName, message, cancellationToken);

				_logger.LogInformation($"Invoice '{request.InvoiceNumber}' conferência message published successfully");

				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error publishing conferência message for invoice '{request.InvoiceNumber}': {ex.Message}", ex);
				throw;
			}
		}
	}
}
