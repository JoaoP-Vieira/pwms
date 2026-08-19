using System.Text.Json;
using RabbitMQ.Client;
using PWMS.Core.Interfaces;
using PWMS.Service.RabbitMQ.Interfaces;

namespace PWMS.Service.RabbitMQ
{
	public class RabbitMQPublisher : IRabbitMQPublisher
	{
		private readonly IConnection _connection;
		private readonly IApplicationLogger _logger;

		public RabbitMQPublisher(IConnection connection, IApplicationLogger logger)
		{
			_connection = connection;
			_logger = logger;
		}

		public async Task PublishAsync<T>(string queueName, string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
		{
			try
			{
				using var channel = await _connection.CreateChannelAsync();

				await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true, autoDelete: false);

				await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

				await channel.QueueBindAsync(queueName, exchangeName, routingKey);

				var jsonMessage = JsonSerializer.Serialize(message);
				var body = System.Text.Encoding.UTF8.GetBytes(jsonMessage);

				await channel.BasicPublishAsync(exchangeName, routingKey, body);

				_logger.LogInformation($"Message published to queue '{queueName}': {jsonMessage}");
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error publishing message to queue '{queueName}': {ex.Message}", ex);
				throw;
			}
		}
	}
}
