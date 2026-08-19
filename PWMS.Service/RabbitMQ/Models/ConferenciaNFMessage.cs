namespace PWMS.Service.RabbitMQ.Models
{
	public class ConferenciaNFMessage
	{
		public string InvoiceNumber { get; set; } = string.Empty;
		public Guid InvoiceId { get; set; }
		public int UserId { get; set; }
		public DateTime ConferredAt { get; set; }
	}
}
