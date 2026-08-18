using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities.Pendency
{
	public class PendencyItem : Item
	{
		public PendencyType PendencyType { get; private set; }
		public string Description { get; private set; }
		public Guid InvoiceId { get; private set; }
		public int InvoiceLineNumber { get; private set; }
		public Location? Location { get; private set; }
		public PendencyStatus Status { get; private set; }
		public int CreatedByUserId { get; private set; }
		public int? ResolvedByUserId { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? ResolvedAt { get; private set; }

		public PendencyItem(
			Material material, 
			PendencyType pendencyType, 
			string description,
			Guid invoiceId,
			int invoiceLineNumber,
			int createdByUserId,
			Location? location = null) 
			: base(material)
		{
			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Description should be informed", nameof(description));

			if (invoiceLineNumber <= 0)
				throw new ArgumentException("Invoice line number should be valid", nameof(invoiceLineNumber));

			if (createdByUserId <= 0)
				throw new ArgumentException("Created by user ID should be valid", nameof(createdByUserId));

			PendencyType = pendencyType;
			Description = description;
			InvoiceId = invoiceId;
			InvoiceLineNumber = invoiceLineNumber;
			Location = location;
			Status = PendencyStatus.Open;
			CreatedByUserId = createdByUserId;
			ResolvedByUserId = null;
			CreatedAt = DateTime.Now;
			ResolvedAt = null;
		}

		public void UpdateLocation(Location newLocation)
		{
			if (Status != PendencyStatus.Open && Status != PendencyStatus.InAnalysis)
				throw new InvalidOperationException("Cannot update location of resolved or cancelled pendency");

			Location = newLocation;
		}

		public void UpdateStatus(PendencyStatus newStatus, int? resolvedByUserId = null)
		{
			if (newStatus == Status)
				return;

			if (newStatus == PendencyStatus.Resolved || newStatus == PendencyStatus.Cancelled)
			{
				if (!resolvedByUserId.HasValue)
					throw new ArgumentException("Resolved by user ID should be informed", nameof(resolvedByUserId));

				ResolvedByUserId = resolvedByUserId;
				ResolvedAt = DateTime.Now;
			}

			Status = newStatus;
		}
	}
}
