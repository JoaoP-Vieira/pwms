using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities
{
	public abstract class Item
	{
		public Guid Id { get; }
		public Material Material { get; private set; }
		public Location Location { get; private set; }
	}
}
