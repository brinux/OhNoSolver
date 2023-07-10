namespace brinux.ohnosolver
{
	public class OhNoSchemaSolver
	{
		private OhNoSchema _schema;
		private OhNoCellSolver _cellSolver;

		public OhNoSchemaSolver(OhNoSchema schema)
		{
			_schema = schema;
			_cellSolver = new OhNoCellSolver();
		}

		// This is just to enable mocked tests without introducing D.I.; sorry for the lazyness! :D
        public OhNoSchemaSolver(OhNoSchema schema, OhNoCellSolver cellSolver)
        {
            _schema = schema;
            _cellSolver = cellSolver;
        }

        public bool Solve()
		{
			if (!_schema.IsSolved())
			{
				for (int r = 0; r < _schema.Cells.Length; r++)
				{
					for (int c = 0; c < _schema.Cells[0].Length; c++)
					{
						if (_schema.Cells[r][c].HasValue && !_schema.Cells[r][c].IsSolved)
						{
							if (_cellSolver.SolveCell(new OhNoCellCoordinate(r, c, _schema)))
							{
								Console.WriteLine();

								return true;
							}
						}
					}
				}
			}
			else if (!_schema.IsCompleted())
			{
				for (int r = 0; r < _schema.Cells.Length; r++)
				{
					for (int c = 0; c < _schema.Cells[0].Length; c++)
					{
						if (_schema.Cells[r][c].IsEmpty)
						{
							_schema.Cells[r][c].Status = OhNoCellStatusEnum.Blocked;

							Console.WriteLine($"Cell {r+1}:{c+1} was blocked since it was not relevant");
							Console.WriteLine();
						}
					}
				}

				return true;
			}

			return false;
		}
	}
}