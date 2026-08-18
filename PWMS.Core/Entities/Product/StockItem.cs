using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities.Product
{
	public class StockItem : Item
	{
		public string Label { get; private set; }
		public StockLocation Location { get; private set; }
		public Guid InvoiceId { get; private set; }
		public int InvoiceLineNumber { get; private set; }
		public int ConferenceUserId { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? ModifiedAt { get; private set; }

		public StockItem(
			Material material, 
			string label, 
			LabelType type, 
			StockLocation location,
			Guid invoiceId,
			int invoiceLineNumber,
			int conferenceUserId)
			: base(material)
		{
			if (string.IsNullOrWhiteSpace(label))
				throw new ArgumentException("Label should be informed", nameof(label));

			if (invoiceLineNumber <= 0)
				throw new ArgumentException("Invoice line number should be valid", nameof(invoiceLineNumber));

			if (conferenceUserId <= 0)
				throw new ArgumentException("Conference user ID should be valid", nameof(conferenceUserId));

			Label = label;
			Type = type;
			Location = location;
			InvoiceId = invoiceId;
			InvoiceLineNumber = invoiceLineNumber;
			ConferenceUserId = conferenceUserId;
			CreatedAt = DateTime.Now;
			ModifiedAt = null;
		}

		public void UpdateLocation(StockLocation newLocation)
		{
			Location = newLocation ?? throw new ArgumentNullException(nameof(newLocation));
			ModifiedAt = DateTime.Now;
		}
	}
}
