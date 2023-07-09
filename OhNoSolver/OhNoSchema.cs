namespace brinux.ohnosolver
{
	public class OhNoSchema
	{
		public OhNoCell[][] Cells { get; private set; }

		public OhNoSchema(int heigth, int length)
		{
			Cells = new OhNoCell[heigth][];

			for (int r = 0; r < heigth; r++)
			{
				Cells[r] = new OhNoCell[length];

				for (int c = 0; c < length; c++)
				{
					Cells[r][c] = new OhNoCell();
				}
			}
		}

		public OhNoSchema(int heigth, int length, List<OhNoCellSetup> setup) : this(heigth, length)
		{
			foreach (var cell in setup)
			{
				if (cell.Row >= Cells.Length)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Cells.Length }x{ Cells[0].Length } ).");
				}
				else if (cell.Column >= Cells[0].Length)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Cells.Length }x{ Cells[0].Length } ).");
				}

				switch (cell.Status)
				{
					case OhNoCellStatusEnum.Full:
						Cells[cell.Row][cell.Column] = new OhNoCell(cell.Value, OhNoCellStatusEnum.Full);
						break;

					case OhNoCellStatusEnum.Blocked:
						Cells[cell.Row][cell.Column] = new OhNoCell(OhNoCellStatusEnum.Blocked);
						break;

					default:
						throw new ArgumentException("Empty cells cannot be setup.");
				}
			}
		}

		public bool IsSolved()
		{
            for (int r = 0; r < Cells.Length; r++)
            {
                for (int c = 0; c < Cells[0].Length; c++)
                {
                    if (Cells[r][c].HasValue && !Cells[r][c].IsSolved)
                    {
						return false;
                    }
                }
            }

			return true;
        }

        public bool IsCompleted()
        {
			if (!IsSolved())
			{
				return false;
			}

            for (int r = 0; r < Cells.Length; r++)
            {
                for (int c = 0; c < Cells[0].Length; c++)
                {
                    if (Cells[r][c].IsEmpty)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
