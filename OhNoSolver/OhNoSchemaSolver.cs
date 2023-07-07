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

			var fullOnTop = CountSequenceLength(OhNoDirectionEnum.TOP, (r, c)) - 1;
			var fullOnRight = CountSequenceLength(OhNoDirectionEnum.RIGHT, (r, c)) - 1;
            var fullOnBottom = CountSequenceLength(OhNoDirectionEnum.BOTTOM, (r, c)) - 1;
            var fullOnLeft = CountSequenceLength(OhNoDirectionEnum.LEFT, (r, c)) - 1;

            var fullTotal = fullOnTop + fullOnRight + fullOnBottom + fullOnLeft;

			if (fullTotal == _schema.Cells[r][c].Value)
			{
				// blocca cella

				return result;
			}

			var availableOnTop = CountAvailableCells(OhNoDirectionEnum.TOP, (r, c), fullTotal);
			var availableOnRight = CountAvailableCells(OhNoDirectionEnum.RIGHT, (r, c), fullTotal);
            var availableOnBottom = CountAvailableCells(OhNoDirectionEnum.BOTTOM, (r, c), fullTotal);
            var availableOnLeft = CountAvailableCells(OhNoDirectionEnum.LEFT, (r, c), fullTotal);

            var availableTotal = availableOnTop + availableOnRight + availableOnBottom + availableOnLeft;

			Console.WriteLine($"Cell {r+1}:{c+1} - Count: {fullOnTop}, {fullOnRight}, {fullOnBottom}, {fullOnLeft} - Available: {availableOnTop}, {availableOnRight}, {availableOnBottom}, {availableOnLeft}");

			if (availableTotal == _schema.Cells[r][c].Value - fullTotal)
			{
				// riempi tutte
				Console.WriteLine("All available cells should be filled");

				result = true;
			}
			else if (availableTotal < _schema.Cells[r][c].Value - fullTotal)
			{
				throw new Exception($"Error: it is not possible to satisfy cell {r + 1}:{c + 1} requirement of {_schema.Cells[r][c].Value}.");
			}
			else
			{
				// Expansion on top
				if (availableOnRight + availableOnBottom + availableOnLeft < _schema.Cells[r][c].Value && availableOnTop > 0)
				{
                    // expand on top
                    Console.WriteLine($"Should expand on top - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnTop}");
                }

                // Expansion on the right
                if (availableOnBottom + availableOnLeft + availableOnTop < _schema.Cells[r][c].Value && availableOnRight > 0)
                {
                    // expand on right
                    Console.WriteLine($"Should expand on right - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnRight}");
                }

                // Expansion on bottom
                if (availableOnLeft + availableOnTop + availableOnRight < _schema.Cells[r][c].Value && availableOnBottom > 0)
                {
                    // expand on bottom
                    Console.WriteLine($"Should expand on bottom - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnBottom}");
                }

                // Expansion on the left
                if (availableOnTop + availableOnRight + availableOnBottom < _schema.Cells[r][c].Value && availableOnLeft > 0)
                {
                    // expand on left
                    Console.WriteLine($"Should expand on left - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnLeft}");
                }
            }

			Console.ReadKey();

			return result;
		}

		private int CountAvailableCells(OhNoDirectionEnum direction, (int row, int column) startingCell, int currentCellsCount)
		{
			var cell = startingCell;
			int count = 0;
			var availabilities = new List<((int row, int column) cell, int steps)>();

			while (CanProceed(direction, cell))
			{
				cell = Move(direction, cell);

				if (_schema.Cells[cell.row][cell.column].Status == OhNoCellStatusEnum.Empty)
				{
					break;
				}
            }

			if (_schema.Cells[cell.row][cell.column].IsEmpty)
			{
				do
				{
					// C'è una cella dopo?
					if (!CanProceed(direction, cell))
					{
						// No: +1, ed esco
						count++;
						availabilities.Add(((cell.row, cell.column), 1));
						return count;
					}
					else
					{
						// Si: diche tipo è?
						var nextCell = Move(direction, cell);

						switch (_schema.Cells[nextCell.row][nextCell.column].Status)
						{
							// Vuota: +1 e procedi
							case OhNoCellStatusEnum.Empty:
								count += 1;
								availabilities.Add(((cell.row, cell.column), 1));

								cell = nextCell;
								break;
							// Blocco: +1 ed esci
							case OhNoCellStatusEnum.Blocked:
								count += 1;
								availabilities.Add(((cell.row, cell.column), 1));

								return count;
							// Full: quanto è lunga la sequenza, compresa la cella correte?
							case OhNoCellStatusEnum.Full:
								int length = CountSequenceLength(direction, nextCell);

								// Quanto il target: +1 + lunghezza sequenza ed esco
								if (length + currentCellsCount == _schema.Cells[startingCell.row][startingCell.column].Value)
								{
									availabilities.Add(((cell.row, cell.column), length + 1));
									return count + length + 1;
								}
								// Meno del target: +1 +lunghezza sequenza e procedo dalla fine della sequenza							
								else if (length + currentCellsCount < _schema.Cells[startingCell.row][startingCell.column].Value)
								{
									availabilities.Add(((cell.row, cell.column), length + 1));
									count += length + 1;

									if (CanProceed(direction, nextCell, length))
									{
										cell = Move(direction, nextCell, length);
									}
									else
									{
										return count;
									}
								}
								// Più del target: esco
								else
								{
									// TODO: devo bloccare la cella corrente?
									return count;
								}

								break;
						}
					}
				} while (true);
			}

			return count;
		}

		private bool CanProceed(OhNoDirectionEnum direction, (int row, int column) cell, int steps = 1)
		{
			switch (direction)
			{
				case OhNoDirectionEnum.TOP:
					return cell.row - steps >= 0;
				case OhNoDirectionEnum.BOTTOM:
					return cell.row + steps < _schema.Cells.Length;
				case OhNoDirectionEnum.LEFT:
					return cell.column - steps >= 0;
				case OhNoDirectionEnum.RIGHT:
					return cell.column + steps < _schema.Cells.Length;
			}

			throw new NotImplementedException($"Missing implementation for direction: {direction}");
		}

		private (int row, int column) Move(OhNoDirectionEnum direction, (int row, int column) cell, int steps = 1)
		{
            switch (direction)
            {
                case OhNoDirectionEnum.TOP:
                    return (cell.row - steps, cell.column);
                case OhNoDirectionEnum.BOTTOM:
                    return (cell.row + steps, cell.column);
                case OhNoDirectionEnum.LEFT:
                    return (cell.row, cell.column - steps);
                case OhNoDirectionEnum.RIGHT:
                    return (cell.row, cell.column + steps);
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }

        private int CountSequenceLength(OhNoDirectionEnum direction, (int row, int column) cell)
        {
			var currentCell = cell;
			int count = 0;

			while (_schema.Cells[currentCell.row][currentCell.column].Status == OhNoCellStatusEnum.Full)
			{
				count++;

				if (CanProceed(direction, currentCell))
				{
					currentCell = Move(direction, currentCell);
				}
				else
				{
					break;
				}
			}

			return count;
        }
    }
}