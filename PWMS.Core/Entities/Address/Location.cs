namespace PWMS.Core.Entities.Address
{
	public class Location
	{
		public int Id { get; private set; }
		public string Identification { get; private set; }
		public Zone Zone { get; private set; }
		public bool IsLocked { get; private set; }

		private Location() { }

		protected Location(string identification, Zone zone, bool isLocked)
		{
			Identification = identification;
			Zone = zone;
			IsLocked = isLocked;
		}

		protected virtual string GetAddress()
		{
			return Identification;
		}
	}
}
