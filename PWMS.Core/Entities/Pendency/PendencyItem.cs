namespace PWMS.Core.Entities.Pendency
{
	public class PendencyItem : Item
	{
		public PendencyType PendencyType { get; private set; }
		public string Description { get; private set; }

		public PendencyItem(PendencyType pendencyType, string description)
		{
			PendencyType = pendencyType;
			Description = description;
		}
	}
}
