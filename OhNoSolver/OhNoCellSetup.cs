namespace brinux.ohnosolver
{
	public class OhNoCellSetup
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public OhNoCellStatusEnum Status { get; set; }
		public int Value { get; set; }

		private OhNoCellSetup() {}

		public static OhNoCellSetup SetupBlockedCell(int row, int column)
		{
			return new OhNoCellSetup()
			{
				Row = row - 1,
				Column = column - 1,
				Status = OhNoCellStatusEnum.Blocked
			};
		}

		public static OhNoCellSetup SetupFullCell(int row, int column, int value)
		{
			if (value < 0)
			{
				throw new ArgumentException("A full cell can only be set up with a positive value.");
			}

			return new OhNoCellSetup()
			{
				Row = row - 1,
				Column = column - 1,
				Status = OhNoCellStatusEnum.Full,
				Value = value
			};
		}
	}
}
