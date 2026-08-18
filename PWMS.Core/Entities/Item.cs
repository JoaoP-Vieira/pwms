using PWMS.Core.Entities.Address;

namespace PWMS.Core.Entities
{
	public abstract class Item
	{
		public int Id { get; private set; }
		public Material Material { get; private set; }

		protected Item(Material material)
		{
			Material = material;
		}
	}
}
