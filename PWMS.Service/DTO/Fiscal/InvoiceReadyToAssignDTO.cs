namespace PWMS.Service.DTO.Fiscal
{
	public class InvoiceReadyToAssignDTO
	{
		public string InvoiceNumber { get; set; } = string.Empty;
		public string Series { get; set; } = string.Empty;
		public DateTime IssueDate { get; set; }
		public decimal TotalAmount { get; set; }
		public int TotalVolumes { get; set; }
		public int TotalItens { get; set; }
		public DateTime CreatedAt { get; set; }
		public string Issuer { get; set; } = string.Empty;
		public string Recipient { get; set; } = string.Empty;
		public string Carrier { get; set; } = string.Empty;
	}
}
