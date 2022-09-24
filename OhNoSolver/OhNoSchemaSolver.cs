namespace brinux.ohnosolver
{
	public class OhNoSchemaSolver
	{
		private OhNoSchema _schema;

		public OhNoSchemaSolver(OhNoSchema schema)
		{
			_schema = schema;
		}

		public bool Solve()
		{
			bool result = false;

			for (int r = 0; r < _schema.Cells.Length; r++)
			{
				for (int c = 0; c < _schema.Cells[0].Length; c++)
				{
					if (_schema.Cells[r][c].HasValue)
					{
						result = SolveCell(r, c);
					}
				}
			}

			return result;
		}

		public bool SolveCell(int r, int c)
		{
			var result = false;

			var fullOnTop = CountOnTop(r, c, c => c.Status == OhNoCellStatusEnum.Full);
			var fullOnRight = CountOnRight(r, c, c => c.Status == OhNoCellStatusEnum.Full);
			var fullOnBottom = CountOnBottom(r, c, c => c.Status == OhNoCellStatusEnum.Full);
			var fullOnLeft = CountOnLeft(r, c, c => c.Status == OhNoCellStatusEnum.Full);

			var fullTotal = fullOnTop + fullOnRight + fullOnBottom + fullOnLeft;

			if (fullTotal == _schema.Cells[r][c].Value)
			{
				// blocca cella

				return result;
			}

			var availableOnTop = CountOnTop(r, c, c => c.Status != OhNoCellStatusEnum.Blocked);
			var availableOnRight = CountOnRight(r, c, c => c.Status != OhNoCellStatusEnum.Blocked);
			var availableOnBottom = CountOnBottom(r, c, c => c.Status != OhNoCellStatusEnum.Blocked);
			var availableOnLeft = CountOnLeft(r, c, c => c.Status != OhNoCellStatusEnum.Blocked);

			var availableTotal = availableOnTop + availableOnRight + availableOnBottom + availableOnLeft;

			if (availableTotal == _schema.Cells[r][c].Value)
			{
				// riempi tutte

				result = true;
			}

			return result;
		}

		private int CountAround(int r, int c, Func<OhNoCell, bool> criteria)
		{
			return
				CountOnTop(r, c, criteria) +
				CountOnRight(r, c, criteria) +
				CountOnBottom(r, c, criteria) +
				CountOnLeft(r, c, criteria);
		}

		private int CountOnTop(int r, int c, Func<OhNoCell, bool> criteria)
		{
			int count = 0;

			for (int i = r - 1; i >= 0; i--)
			{
				if (criteria(_schema.Cells[i][c]))
				{
					count++;
				}
				else
				{
					break;
				}
			}

			return count;
		}

		private int CountOnBottom(int r, int c, Func<OhNoCell, bool> criteria)
		{
			int count = 0;

			for (int i = r + 1; i < _schema.Cells.Length; i++)
			{
				if (criteria(_schema.Cells[i][c]))
				{
					count++;
				}
				else
				{
					break;
				}
			}

			return count;
		}

		private int CountOnRight(int r, int c, Func<OhNoCell, bool> criteria)
		{
			int count = 0;

			for (int i = c + 1; i < _schema.Cells[0].Length; i++)
			{
				if (criteria(_schema.Cells[r][i]))
				{
					count++;
				}
				else
				{
					break;
				}
			}

			return count;
		}

		private int CountOnLeft(int r, int c, Func<OhNoCell, bool> criteria)
		{
			int count = 0;

			for (int i = c - 1; i >= 0; i--)
			{
				if (criteria(_schema.Cells[r][i]))
				{
					count++;
				}
				else
				{
					break;
				}
			}

			return count;
		}

		public int CheckAvailabilityOnLeft(int r, int c)
		{
			var sourceCellConnections = CountAround(r, c, cell => cell.IsFull);

			int count = 0;

			for (int i = c - 1; i >= 0; i--)
			{
				if (_schema.Cells[r][i].IsBlocked)
				{
					break;
				}
				else
				{
					if (i == 0)
					{
						if (!_schema.Cells[r][i].IsFull)
						{
							count++;
						}
					}
					else if (i - 1 >= 0)
					{
						if (!_schema.Cells[r][i - 1].IsFull)
						{
							count++;
						}
						else
						{
							var newConnections = CountOnLeft(r, i, c => c.IsFull);

							var targetCellConnections = CountAround(r, i - 1, cell => cell.IsFull);

							if (newConnections + count + 1 + sourceCellConnections > _schema.Cells[r][c].Value || (
									_schema.Cells[r][i - 1].HasValue &&
									CountAround(r, i - 1, cell => cell.IsFull) + count + 2 > _schema.Cells[r][i - 1].Value))
							{
								if (count == 0)
								{
									_schema.Cells[r][i].Status = OhNoCellStatusEnum.Blocked;

									Console.WriteLine($"While checking cells on the lift of { r }:{ c }, it resulted cell { r }:{ i } had to be blocked to avoid connecting too many cells.");
								}

								break;
							}
							else
							{
								count++;
							}
						}
					}
				}
			}

			return count;
		}
	}
}