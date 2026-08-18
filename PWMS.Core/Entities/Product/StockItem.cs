using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities.Product
{
	public class StockItem : Item
	{
		public string Label { get; private set; }
		public LabelType Type { get; private set; }
		public StockLocation Location { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime? ModifiedAt { get; private set; }

		public StockItem(Material material, string label, LabelType type, StockLocation location)
			: base(material)
		{
			Label = label;
			Type = type;
			Location = location;
			CreatedAt = DateTime.Now;
			ModifiedAt = null;
		}
	}
}
