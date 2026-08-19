using RabbitMQ.Client;
using Serilog;

namespace PWMS.Service.RabbitMQ
{
	public class RabbitMQConnection : IRabbitMQConnection, IDisposable
	{
		private IConnection _connection { get; set; }
		private readonly ILogger _logger;

		public RabbitMQConnection(string hostName, int port, string userName, string password, ILogger logger)
		{
			_logger = logger;
			CreateConnection(hostName, port, userName, password).GetAwaiter().GetResult();
		}

		public async Task CreateConnection(string hostName, int port, string userName, string password)
		{
			try
			{
				var factory = new ConnectionFactory
				{
					HostName = hostName,
					Port = port,
					UserName = userName,
					Password = password,
					AutomaticRecoveryEnabled = true,
					NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
				};

				_connection = await factory.CreateConnectionAsync();
				_logger.Information("RabbitMQ connection established successfully");
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "Error establishing RabbitMQ connection: {Message}", ex.Message);
				throw;
			}
		}

		public IConnection GetConnection()
		{
			return _connection;
		}

		public void Dispose()
		{
			_connection?.Dispose();
		}
	}
}
