namespace PWMS.Core.Entities
{
	public sealed class Category
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		private Category() { }

		public Category(string name, string description)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Category name should be informed", nameof(name));

			if (string.IsNullOrWhiteSpace(description))
				throw new ArgumentException("Category description should be informed", nameof(description));

			Name = name;
			Description = description;
		}
	}
}
