namespace PWMS.Core.Entities.Pendency
{
	public class PendencyItem : Item
	{
		public PendencyType PendencyType { get; private set; }
		public string Description { get; private set; }

		public PendencyItem(Material material, PendencyType pendencyType, string description) : base(material)
		{
			PendencyType = pendencyType;
			Description = description;
		}
	}
}
