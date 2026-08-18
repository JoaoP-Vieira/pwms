using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities.Fiscal
{
	public sealed class Invoice
	{
		public Guid Id { get; private set; }
		public string InvoiceNumber { get; private set; }
		public string Series { get; private set; }
		public string VerificationCode { get; private set; }
		public InvoiceType Type { get; private set; }
		public InvoiceStatus Status { get; private set; }
		public Person Issuer { get; private set; }
		public Person Recipient { get; private set; }
		public Person? Carrier { get; private set; }
		public DateTime IssueDate { get; private set; }
		public DateTime? ExpectedDeliveryDate { get; private set; }
		public decimal TotalAmount { get; private set; }
		public int TotalVolumes { get; private set; }
		public decimal TotalGrossWeight { get; private set; }
		public List<InvoiceItem> Items { get; private set; } = new List<InvoiceItem>();
		public DateTime CreatedAt { get; private set; }
		public DateTime? UpdatedAt { get; private set; }
		public Location? ConferenceLocation { get; private set; }
		public string? AssingVehicle { get; private set; }

		private Invoice() { }

		public Invoice(string invoiceNumber, string series, string verificationCode, int type, Person issuer,
			Person recipient, Person? carrier, DateTime issueDate, DateTime? expectedDeliveryDate, decimal totalAmount,
			int totalVolumes, decimal totalGrossWeight, List<InvoiceItem> items, DateTime createdAt, DateTime? updatedAt)
		{
			if (string.IsNullOrWhiteSpace(invoiceNumber))
				throw new ArgumentException("Invoice number should be informed", "invoiceNumber");

			if (string.IsNullOrWhiteSpace(series))
				throw new ArgumentException("Series should be informed", "series");

			if (!Enum.IsDefined(typeof(InvoiceType), type))
				throw new ArgumentException("Invalid type for creation", "type");

			if (!ValidateInvoice(verificationCode))
				throw new ArgumentException("Invalid verification code", "verificationCode");

			if (totalAmount <= 0)
				throw new ArgumentException("Invalid total amount", "totalAmount");

			if (totalVolumes <= 0)
				throw new ArgumentException("Invalid total volumes", "totalVolumes");

			if (totalGrossWeight <= 0)
				throw new ArgumentException("Invalid total gross weight", "totalGrossWeight");

			if (items.Count == 0)
				throw new ArgumentException("Invoice items should be informed", "items");

			Id = Guid.NewGuid();
			InvoiceNumber = invoiceNumber;
			Series = series;
			VerificationCode = verificationCode;
			Type = (InvoiceType)type;
			Status = InvoiceStatus.Created;
			Issuer = issuer;
			Recipient = recipient;
			Carrier = carrier;
			IssueDate = issueDate;
			ExpectedDeliveryDate = expectedDeliveryDate;
			TotalAmount = totalAmount;
			TotalVolumes = totalVolumes;
			TotalGrossWeight = totalGrossWeight;
			Items = items;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			ConferenceLocation = null;
			AssingVehicle = null;
		}

		private bool ValidateInvoice(string verificationCode)
		{
			if (string.IsNullOrWhiteSpace(verificationCode) || verificationCode.Length != 44)
				return false;

			string base43 = verificationCode.Substring(0, 43);

			if (!int.TryParse(verificationCode.Substring(43, 1), out int digit))
				return false;

			int sum = 0;
			int multiplier = 2;

			for (int i = base43.Length - 1; i >= 0; i--)
			{
				int num = base43[i] - '0';
				sum += num * multiplier;

				multiplier++;
				if (multiplier > 9)
				{
					multiplier = 2;
				}
			}

			int rest = sum % 11;
			int callculedDigit = 11 - rest;

			if (callculedDigit >= 10)
			{
				callculedDigit = 0;
			}

			return callculedDigit == digit;
		}

		public void SetIssuer(Person person)
		{
			Issuer = person;
		}

		public void SetRecipient(Person person)
		{
			Recipient = person;
		}

		public void SetCarrier(Person? person)
		{
			Carrier = person;
		}

		public void SetConferenceLocation(Location? location)
		{
			ConferenceLocation = location;
		}

		public void CheckStartConference()
		{
			if (Status != InvoiceStatus.Pending && Status != InvoiceStatus.InConference)
				throw new InvalidOperationException("Invoice in another process");

			if (ConferenceLocation == null || ConferenceLocation.Zone != Zone.InboundArea)
				throw new InvalidOperationException("Invoice located in a wrong place");
		}
	}

	public enum InvoiceType
	{
		Normal = 1,
		Complementary = 2,
		Devolution = 3,
		Ajust = 4,
	}

	public enum InvoiceStatus
	{
		Created = 0,
		Pending = 1,
		InConference = 2,
		Processed = 3,
		Canceled = 4
	}
}
