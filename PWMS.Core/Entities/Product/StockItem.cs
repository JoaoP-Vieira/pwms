using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities.Product
{
	public class StockItem : Item
	{
		public string Label { get; private set; }
		public LabelType Type { get; }
		public StockLocation Location { get; private set; }
	}
}
