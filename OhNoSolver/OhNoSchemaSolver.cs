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

			var currentCell = new OhNoCellCoordinate(r, c);

            var fullOnTop = CountSequenceLength(OhNoDirectionEnum.TOP, currentCell) - 1;
			var fullOnRight = CountSequenceLength(OhNoDirectionEnum.RIGHT, currentCell) - 1;
            var fullOnBottom = CountSequenceLength(OhNoDirectionEnum.BOTTOM, currentCell) - 1;
            var fullOnLeft = CountSequenceLength(OhNoDirectionEnum.LEFT, currentCell) - 1;

            var fullTotal = fullOnTop + fullOnRight + fullOnBottom + fullOnLeft;

			if (fullTotal == _schema.Cells[r][c].Value)
			{
				// blocca cella

				return result;
			}

			var availableOnTop = CountAvailableCells(OhNoDirectionEnum.TOP, currentCell, fullTotal);
			var availableOnRight = CountAvailableCells(OhNoDirectionEnum.RIGHT, currentCell, fullTotal);
            var availableOnBottom = CountAvailableCells(OhNoDirectionEnum.BOTTOM, currentCell, fullTotal);
            var availableOnLeft = CountAvailableCells(OhNoDirectionEnum.LEFT, currentCell, fullTotal);

            var availableTotal =
				availableOnTop.CountTotalLength +
				availableOnRight.CountTotalLength +
				availableOnBottom.CountTotalLength +
				availableOnLeft.CountTotalLength;

			Console.WriteLine($"Cell {r+1}:{c+1} - Count: {fullOnTop}, {fullOnRight}, {fullOnBottom}, {fullOnLeft} - Available: {availableOnTop.CountTotalLength}, {availableOnRight.CountTotalLength}, {availableOnBottom.CountTotalLength}, {availableOnLeft.CountTotalLength}");

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
				if (availableOnRight.CountTotalLength + availableOnBottom.CountTotalLength + availableOnLeft.CountTotalLength < _schema.Cells[r][c].Value && availableOnTop.CountMoves > 0)
				{
                    // expand on top
                    Console.WriteLine($"Should expand on top - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnTop.CountTotalLength} in {availableOnTop.CountMoves} moves");
                }

                // Expansion on the right
                if (availableOnBottom.CountTotalLength + availableOnLeft.CountTotalLength + availableOnTop.CountTotalLength < _schema.Cells[r][c].Value && availableOnRight.CountMoves > 0)
                {
                    // expand on right
                    Console.WriteLine($"Should expand on right - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnRight.CountTotalLength} in {availableOnRight.CountMoves} moves");
                }

                // Expansion on bottom
                if (availableOnLeft.CountTotalLength + availableOnTop.CountTotalLength + availableOnRight.CountTotalLength < _schema.Cells[r][c].Value && availableOnBottom.CountMoves > 0)
                {
                    // expand on bottom
                    Console.WriteLine($"Should expand on bottom - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnBottom.CountTotalLength} in {availableOnBottom.CountMoves} moves");
                }

                // Expansion on the left
                if (availableOnTop.CountTotalLength + availableOnRight.CountTotalLength + availableOnBottom.CountTotalLength < _schema.Cells[r][c].Value && availableOnLeft.CountMoves > 0)
                {
                    // expand on left
                    Console.WriteLine($"Should expand on left - Current: {fullTotal} - Target: {_schema.Cells[r][c].Value} - Available: {availableOnLeft.CountTotalLength} in {availableOnLeft.CountMoves} moves");
                }
            }

			Console.ReadKey();

			return result;
		}

		private OhNoCellMoves CountAvailableCells(OhNoDirectionEnum direction, OhNoCellCoordinate startingCell, int currentCellsCount)
		{
			var cell = startingCell;
			var availabilities = new OhNoCellMoves();

			while (CanProceed(direction, cell))
			{
				cell = Move(direction, cell);

				if (_schema.Cells[cell.Row][cell.Column].Status == OhNoCellStatusEnum.Empty)
				{
					break;
				}
            }

			if (_schema.Cells[cell.Row][cell.Column].IsEmpty)
			{
				do
				{
					// C'è una cella dopo?
					if (!CanProceed(direction, cell))
					{
						// No: +1, ed esco
						availabilities.Add(new OhNoCellMove(cell, 1));

						return availabilities;
					}
					else
					{
						// Si: diche tipo è?
						var nextCell = Move(direction, cell);

						switch (_schema.Cells[nextCell.Row][nextCell.Column].Status)
						{
							// Vuota: +1 e procedi
							case OhNoCellStatusEnum.Empty:
								availabilities.Add(new OhNoCellMove(cell, 1));

								cell = nextCell;
								break;
							// Blocco: +1 ed esci
							case OhNoCellStatusEnum.Blocked:
								availabilities.Add(new OhNoCellMove(cell, 1));

								return availabilities;
							// Full: quanto è lunga la sequenza, compresa la cella correte?
							case OhNoCellStatusEnum.Full:
								int length = CountSequenceLength(direction, nextCell);

								// Quanto il target: +1 + lunghezza sequenza ed esco
								if (length + currentCellsCount == _schema.Cells[startingCell.Row][startingCell.Column].Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									return availabilities;
								}
								// Meno del target: +1 +lunghezza sequenza e procedo dalla fine della sequenza							
								else if (length + currentCellsCount < _schema.Cells[startingCell.Row][startingCell.Column].Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									if (CanProceed(direction, nextCell, length))
									{
										cell = Move(direction, nextCell, length);
									}
									else
									{
										return availabilities;
									}
								}
								// Più del target: esco
								else
								{
									// TODO: devo bloccare la cella corrente?
									return availabilities;
								}

								break;
						}
					}
				} while (true);
			}

			return availabilities;
		}

		private bool CanProceed(OhNoDirectionEnum direction, OhNoCellCoordinate cell, int steps = 1)
		{
			switch (direction)
			{
				case OhNoDirectionEnum.TOP:
					return cell.Row - steps >= 0;
				case OhNoDirectionEnum.BOTTOM:
					return cell.Row + steps < _schema.Cells.Length;
				case OhNoDirectionEnum.LEFT:
					return cell.Column - steps >= 0;
				case OhNoDirectionEnum.RIGHT:
					return cell.Column + steps < _schema.Cells.Length;
			}

			throw new NotImplementedException($"Missing implementation for direction: {direction}");
		}

		private OhNoCellCoordinate Move(OhNoDirectionEnum direction, OhNoCellCoordinate cell, int steps = 1)
		{
            switch (direction)
            {
                case OhNoDirectionEnum.TOP:
                    return new OhNoCellCoordinate(cell.Row - steps, cell.Column);
                case OhNoDirectionEnum.BOTTOM:
                    return new OhNoCellCoordinate(cell.Row + steps, cell.Column);
                case OhNoDirectionEnum.LEFT:
                    return new OhNoCellCoordinate(cell.Row, cell.Column - steps);
                case OhNoDirectionEnum.RIGHT:
                    return new OhNoCellCoordinate(cell.Row, cell.Column + steps);
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }

        private int CountSequenceLength(OhNoDirectionEnum direction, OhNoCellCoordinate cell)
        {
			var currentCell = cell;
			int count = 0;

			while (_schema.Cells[currentCell.Row][currentCell.Column].Status == OhNoCellStatusEnum.Full)
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