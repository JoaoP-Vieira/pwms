namespace PWMS.Service.RabbitMQ.Interfaces
{
	public interface IRabbitMQPublisher
	{
		Task PublishAsync<T>(string queueName, string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default) where T : class;
	}
}
