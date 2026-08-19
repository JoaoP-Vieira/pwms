using PWMS.Core.Interfaces;
using RabbitMQ.Client;

namespace PWMS.Service.RabbitMQ
{
	public interface IRabbitMQConnection
	{
		Task CreateConnection(string hostName, int port, string userName, string password);
		IConnection GetConnection();
	}
}
