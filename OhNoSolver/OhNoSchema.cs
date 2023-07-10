namespace brinux.ohnosolver
{
	public class OhNoSchema
	{
		public OhNoCell[][] Cells { get; private set; }

		public int Height { get; private set; }
		public int Width { get; private set; }

        public OhNoSchema(int height, int width)
		{
			Height = height;
			Width = width;

			Cells = new OhNoCell[Height][];

			for (int r = 0; r < Height; r++)
			{
				Cells[r] = new OhNoCell[Width];

				for (int c = 0; c < Width; c++)
				{
					Cells[r][c] = new OhNoCell();
				}
			}
		}

		public OhNoSchema(int height, int width, List<OhNoCellSetup> setup) : this(height, width)
		{
			foreach (var cell in setup)
			{
				if (cell.Row >= Height)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Height }x{ Width }).");
				}
				else if (cell.Column >= Width)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Height }x{ Width }).");
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
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
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

            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
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
