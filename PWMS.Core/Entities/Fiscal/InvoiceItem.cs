namespace PWMS.Core.Entities.Fiscal
{
	public sealed class InvoiceItem
	{
		public Guid InvoiceId { get; private set; }
		public int LineNumber { get; private set; }
		public decimal DeclaredQuantity { get; private set; }
		public decimal? ProcessedQuantity { get; private set; }
		public decimal UnitPrice { get; private set; }
		public Material ItemMaterial { get; private set; }
		public decimal Quantity => ProcessedQuantity != null ? ProcessedQuantity.Value : DeclaredQuantity;
		public decimal TotalPrice => Quantity * UnitPrice;

		private InvoiceItem() { }

		public InvoiceItem(int lineNumber, decimal declaredQuantity, decimal unitPrice, Material itemMaterial)
		{
			if (lineNumber <= 0)
				throw new ArgumentException("Item line number should be valid", "lineNumber");

			if (declaredQuantity <= 0)
				throw new ArgumentException("Item declared quantity should be valid", "declaredQuantity");

			if (unitPrice <= 0)
				throw new ArgumentException("Item unit price should be valid", "unitPrice");

			LineNumber = lineNumber;
			DeclaredQuantity = declaredQuantity;
			ProcessedQuantity = null;
			UnitPrice = unitPrice;
			ItemMaterial = itemMaterial;
		}
	}
}
