namespace PWMS.Core.Entities
{
	public sealed class Material
	{
		public Guid Id { get; private set; }
		public string SKU { get; private set; }
		public string Barcode { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public Category Category { get; private set; }
		public decimal Weight { get; private set; }
		public decimal Height { get; private set; }
		public decimal Width { get; private set; }
		public decimal Length { get; private set; }
		public decimal Volume => Height * Width * Length;
		public MaterialStatus Status { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? UpdatedAt { get; private set; }

		private Material() { }

		public Material(string sku, string barcode, string name, string description, Category category, decimal weight, decimal height, decimal width, decimal length)
		{
			if (string.IsNullOrWhiteSpace(sku))
				throw new ArgumentException("Material SKU should be informed", "sku");

			if (string.IsNullOrWhiteSpace(barcode))
				throw new ArgumentException("Material barcode should be informed", "barcode");

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Material name should be informed", "name");

			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Material description should be informed", "description");

			if (weight <= 0)
				throw new ArgumentException("Material weight should be valid", "weight");

			if (height <= 0)
				throw new ArgumentException("Material height should be valid", "height");

			if (width <= 0)
				throw new ArgumentException("Material width should be valid", "width");

			if (length <= 0)
				throw new ArgumentException("Material length should be valid", "length");

			Id = Guid.NewGuid();
			SKU = sku;
			Barcode = barcode;
			Name = name;
			Description = description;
			Category = category;
			Weight = weight;
			Height = height;
			Width = width;
			Length = length;
			Status = MaterialStatus.Enabled;
			CreatedAt = DateTime.UtcNow;
		}

		public void SetCategory(Category category)
		{
			Category = category;
		}
	}

	public enum MaterialStatus
	{
		Disabled = 0,
		Enabled = 1,
		Blocked = 2,
	}
}
