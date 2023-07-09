namespace brinux.ohnosolver
{
	public class OhNoCell
	{
		public OhNoCellStatusEnum Status { get; set; } = OhNoCellStatusEnum.Empty;

		public bool HasValue => Value > 0;
		public int Value { get; private set; } = 0;

		public bool IsSolved { get; set; } = false;

		public bool IsEmpty => Status == OhNoCellStatusEnum.Empty;
		public bool IsBlocked => Status == OhNoCellStatusEnum.Blocked;
		public bool IsFull => Status == OhNoCellStatusEnum.Full;

		public bool WasSetUp { get; private set; } = false;

		public OhNoCell() {}

		public OhNoCell(OhNoCellStatusEnum status = OhNoCellStatusEnum.Blocked)
		{
			if (status == OhNoCellStatusEnum.Empty)
			{
				throw new ArgumentException("Empty cells are created by default and cannot be set up.");
			}
			else if (status == OhNoCellStatusEnum.Full)
			{
				throw new ArgumentException("Full cells must be set up with a value.");
			}

			Status = status;
			WasSetUp = true;
		}

		public OhNoCell(int value, OhNoCellStatusEnum status = OhNoCellStatusEnum.Full)
		{
			if (status == OhNoCellStatusEnum.Empty)
			{
				throw new ArgumentException("Empty cells are created by default and cannot be set up.");
			}
			else if (status == OhNoCellStatusEnum.Blocked)
			{
				throw new ArgumentException("Blocked cells must be set up without a value.");
			}

			Status = status;
			Value = value;
			WasSetUp = true;
		}
	}
}
