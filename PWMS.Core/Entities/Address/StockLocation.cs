namespace PWMS.Core.Entities.Address
{
	public sealed class StockLocation : Location
	{
		public string Aisle { get; private set; }
		public string Column { get; private set; }
		public string Level { get; private set; }
		public string Slot { get; private set; }
		public string Depth { get; private set; }

		public StockLocation(
			string identification,
			Zone zone,
			bool isLocked,
			string aisle,
			string column,
			string level,
			string slot,
			string depth) : base(identification, zone, isLocked)
		{
			if (string.IsNullOrWhiteSpace(aisle))
				throw new ArgumentException("Aisle should be informed", "aisle");

			if (string.IsNullOrWhiteSpace(column))
				throw new ArgumentException("Column should be informed", "column");

			if (string.IsNullOrWhiteSpace(level))
				throw new ArgumentException("Level should be informed", "level");

			if (string.IsNullOrWhiteSpace(slot))
				throw new ArgumentException("Slot should be informed", "slot");

			if (string.IsNullOrWhiteSpace(depth))
				throw new ArgumentException("Depth should be informed", "depth");

			Aisle = aisle;
			Column = column;
			Level = level;
			Slot = slot;
			Depth = depth;
		}

		protected override string GetAddress()
		{
			string prefix = $"{Aisle} {Column}";

			if (!string.IsNullOrWhiteSpace(Slot))
				return $"{prefix} {Slot} {Depth}";

			return $"{prefix} {Depth}";
		}
	}
}
