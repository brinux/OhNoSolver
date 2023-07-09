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
			if (!_schema.IsSolved())
			{
				for (int r = 0; r < _schema.Cells.Length; r++)
				{
					for (int c = 0; c < _schema.Cells[0].Length; c++)
					{
						if (_schema.Cells[r][c].HasValue && !_schema.Cells[r][c].IsSolved)
						{
							if (SolveCell(new OhNoCellCoordinate(r, c, _schema)))
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

		public bool SolveCell(OhNoCellCoordinate currentCell)
		{
			var fullCellsCounts = CountSequenceLengths(currentCell).ToDictionary(i => i.Key, i => i.Value - 1);
			var fullCellsTotal = fullCellsCounts.Sum(l => l.Value);

			var availabilities = CountAvailableCells(currentCell, fullCellsTotal);
			var availabilitiesTotal = availabilities.Sum(a => a.Value.CountTotalLength);

			//Console.WriteLine($"Cell {currentCell.Row}:{currentCell.Column} - Count: {fullCellsCounts[OhNoDirectionEnum.TOP]}, {fullCellsCounts[OhNoDirectionEnum.RIGHT]}, {fullCellsCounts[OhNoDirectionEnum.BOTTOM]}, {fullCellsCounts[OhNoDirectionEnum.LEFT]} - Available: {availabilities[OhNoDirectionEnum.TOP].CountTotalLength}, {availabilities[OhNoDirectionEnum.RIGHT].CountTotalLength}, {availabilities[OhNoDirectionEnum.BOTTOM].CountTotalLength}, {availabilities[OhNoDirectionEnum.LEFT].CountTotalLength}");

			if (availabilitiesTotal < currentCell.Cell.Value - fullCellsTotal)
			{
				throw new Exception($"Error: it is not possible to satisfy cell {currentCell.Row}:{currentCell.Column} requirement of {currentCell.Cell.Value}.");
			}

			if (currentCell.Cell.Value < fullCellsTotal)
			{
				throw new Exception($"Error: cell {currentCell.Row}:{currentCell.Column} has {fullCellsTotal} connected cells, but should have only {currentCell.Cell.Value}");
			}

			if (currentCell.Cell.Value == fullCellsTotal)
			{
				AddFinalBlocks(currentCell);

				return true;
			}

			// Tutte le celle disponibili vanno riempite
			if (availabilitiesTotal == currentCell.Cell.Value - fullCellsTotal)
			{
				FillAllCellAvailabilities(currentCell, availabilities);

				return true;
			}

			// Prova ad aggiungere dei blocchi intorno alla cella
			if (AddMissingBlocks(currentCell, fullCellsTotal))
			{
				return true;
			}

			// Espandi in tutte le direzioni, se possibile
			if (ExpandCell(currentCell, fullCellsTotal, availabilities))
			{
				// Verifica se la cella è completa
				if (fullCellsTotal == currentCell.Cell.Value)
				{
					AddFinalBlocks(currentCell);
                }

                return true;
			}

			return false;
		}

		private void FillAllCellAvailabilities(OhNoCellCoordinate cell, Dictionary<OhNoDirectionEnum, OhNoCellMoves> availabilities)
		{
			foreach (var availabilitiesInDirection in availabilities.Values.ToList())
			{
				if (availabilitiesInDirection.CountMoves > 0)
				{
					foreach (var availability in availabilitiesInDirection)
					{
						availability.Coordinates.Cell.Status = OhNoCellStatusEnum.Full;

						Console.WriteLine($"Cell {availability.Coordinates.Row+1}:{availability.Coordinates.Column+1} was filled since all availabilities for cell {cell.Row+1}:{cell.Column+1} must be to satify its requirement of {cell.Cell.Value} in direction '{availabilitiesInDirection.Direction}'");
					}

					var lastMove = availabilitiesInDirection.Last();

					if (lastMove.Coordinates.CanProceed(availabilitiesInDirection.Direction, lastMove.Length))
					{
						var finalCell = lastMove.Coordinates.Move(availabilitiesInDirection.Direction, lastMove.Length);

						if (finalCell.Cell.IsEmpty)
						{
							finalCell.Cell.Status = OhNoCellStatusEnum.Blocked;

							Console.WriteLine($"Cell {finalCell.Row + 1}:{finalCell.Column + 1} was blocked in direction '{availabilitiesInDirection.Direction}' after adding the required cells for cell {cell.Row + 1}:{cell.Column + 1}");
						}
					}
				}
			}

			AddFinalBlocks(cell);
        }

        private bool ExpandCell(OhNoCellCoordinate currentCell, int fullCellsTotal, Dictionary<OhNoDirectionEnum, OhNoCellMoves> availabilities)
		{
			var result = false;

			foreach (OhNoDirectionEnum direction in Enum.GetValues(typeof(OhNoDirectionEnum)))
			{
				result |= ExpandCellInDirection(currentCell, fullCellsTotal, direction, availabilities);
			}

			return result;
		}

		private bool ExpandCellInDirection(OhNoCellCoordinate currentCell, int fullCellsTotal, OhNoDirectionEnum direction, Dictionary<OhNoDirectionEnum, OhNoCellMoves> availabilities)
		{
			var result = false;
				
			var availabilitiesCountInTheOtherDirections = availabilities.Where(a => a.Key != direction).Sum(a => a.Value.CountTotalLength);

			if (availabilitiesCountInTheOtherDirections + fullCellsTotal < currentCell.Cell.Value && availabilities[direction].CountMoves > 0)
			{
				//Console.WriteLine($"Should expand on top - Current: {fullCellsTotal} - Target: {currentCell.Cell.Value} - Available: {availabilities[direction].CountTotalLength} in {availabilities[direction].CountMoves} moves");

				var missingCoverage = currentCell.Cell.Value - fullCellsTotal - availabilitiesCountInTheOtherDirections;
				var addedCoverage = 0;

				foreach (var availability in availabilities[direction])
				{
					if (missingCoverage >= addedCoverage + 1 /*+ availability.Length*/)
					{
						availability.Coordinates.Cell.Status = OhNoCellStatusEnum.Full;
						addedCoverage += availability.Length;

						Console.WriteLine($"Filled cell {availability.Coordinates.Row+1}:{availability.Coordinates.Column+1} from cell {currentCell.Row+1}:{currentCell.Column+1} in direction '{direction}' - Total length: +{availability.Length}");

						result = true;
					}
					else
					{
						break;
					}
				}
			}

			return result;
		}

		private bool AddFinalBlocks(OhNoCellCoordinate cell)
		{
			var result = false;

			foreach (OhNoDirectionEnum direction in Enum.GetValues(typeof(OhNoDirectionEnum)))
			{
				result |= AddFinalBlockInDirection(direction, cell);
			}

			if (!cell.Cell.IsSolved)
			{
				cell.Cell.IsSolved = true;

				Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} is completed");

				result = true;
			}

            return result;
		}

		private bool AddFinalBlockInDirection(OhNoDirectionEnum direction, OhNoCellCoordinate startingCell)
		{
			var result = false;

			var cell = startingCell;

			while (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (!nextCell.Cell.IsFull)
				{
					break;
				}

				cell = nextCell;
			}

			if (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (nextCell.Cell.IsEmpty)
				{
					nextCell.Cell.Status = OhNoCellStatusEnum.Blocked;

					Console.WriteLine($"Blocked cell {nextCell.Row + 1}:{nextCell.Column + 1} from cell {startingCell.Row + 1}:{startingCell.Column + 1} completion in direction '{direction}'");

					result = true;
				}
			}

			return result;
		}

		private bool AddMissingBlocks(OhNoCellCoordinate cell, int totalCount)
		{
			var result = false;

			foreach (OhNoDirectionEnum direction in Enum.GetValues(typeof(OhNoDirectionEnum)))
			{
				result |= AddMissingBlockInDirection(direction, cell, totalCount);
			}

			return result;
		}

		private bool AddMissingBlockInDirection(OhNoDirectionEnum direction, OhNoCellCoordinate startingCell, int totalCount)
		{
			var result = false;

			var cell = startingCell;

			while (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (!nextCell.Cell.IsFull)
				{
					break;
				}

				cell = nextCell;
			}

			if (cell.CanProceed(direction) && cell.CanProceed(direction, 2))
			{
				var nextCell = cell.Move(direction);
				var followingCell = cell.Move(direction, 2);

				if (nextCell.Cell.IsEmpty && followingCell.Cell.IsFull)
				{
					var sequenceLength = CountSequenceLengthInDirection(direction, followingCell);

					if (startingCell.Cell.Value < totalCount + sequenceLength + 1)
					{
						nextCell.Cell.Status = OhNoCellStatusEnum.Blocked;

						Console.WriteLine($"Blocked cell {nextCell.Row + 1}:{nextCell.Column + 1} from cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}'.");

						result = true;
					}
				}
			}

			return result;
		}

		private Dictionary<OhNoDirectionEnum, OhNoCellMoves> CountAvailableCells(OhNoCellCoordinate startingCell, int currentCellsCount)
		{
			var counts = new Dictionary<OhNoDirectionEnum, OhNoCellMoves>();

			foreach (OhNoDirectionEnum direction in Enum.GetValues(typeof(OhNoDirectionEnum)))
			{
				counts.Add(direction, CountAvailableCellsInDirection(direction, startingCell, currentCellsCount));
			}

			return counts;
		}

		private OhNoCellMoves CountAvailableCellsInDirection(OhNoDirectionEnum direction, OhNoCellCoordinate startingCell, int currentCellsCount)
		{
			var cell = startingCell;
			var availabilities = new OhNoCellMoves(direction);

			while (cell.CanProceed(direction))
			{
				cell = cell.Move(direction);

				if (!cell.Cell.IsFull)
				{
					break;
				}
			}

			if (cell.Cell.IsEmpty)
			{
				do
				{
					// C'è una cella dopo?
					if (!cell.CanProceed(direction))
					{
						// No: +1, ed esco
						availabilities.Add(new OhNoCellMove(cell, 1));

						return availabilities;
					}
					else
					{
						// Si: diche tipo è?
						var nextCell = cell.Move(direction);

						switch (nextCell.Cell.Status)
						{
							// Vuota: +1 e procedi
							case OhNoCellStatusEnum.Empty:
								availabilities.Add(new OhNoCellMove(cell, 1));

								if (currentCellsCount + availabilities.CountTotalLength == startingCell.Cell.Value)
								{
									return availabilities;
								}

								cell = nextCell;
								break;
							// Blocco: +1 ed esci
							case OhNoCellStatusEnum.Blocked:
								availabilities.Add(new OhNoCellMove(cell, 1));

								return availabilities;
							// Full: quanto è lunga la sequenza, compresa la cella correte?
							case OhNoCellStatusEnum.Full:
								int length = CountSequenceLengthInDirection(direction, nextCell);

								// Quanto il target: +1 + lunghezza sequenza ed esco
								if (length + currentCellsCount + availabilities.CountTotalLength == startingCell.Cell.Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									return availabilities;
								}
								// Meno del target: +1 +lunghezza sequenza e procedo dalla fine della sequenza							
								else if (length + currentCellsCount + availabilities.CountTotalLength < startingCell.Cell.Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									if (nextCell.CanProceed(direction, length))
									{
										cell = nextCell.Move(direction, length);
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

		private Dictionary<OhNoDirectionEnum, int> CountSequenceLengths(OhNoCellCoordinate cell)
		{
			var counts = new Dictionary<OhNoDirectionEnum, int>();

			foreach (OhNoDirectionEnum direction in Enum.GetValues(typeof(OhNoDirectionEnum)))
			{
				counts.Add(direction, CountSequenceLengthInDirection(direction, cell));
			}

			return counts;
		}

		private int CountSequenceLengthInDirection(OhNoDirectionEnum direction, OhNoCellCoordinate cell)
		{
			var currentCell = cell;
			int count = 0;

			while (currentCell.Cell.Status == OhNoCellStatusEnum.Full)
			{
				count++;

				if (currentCell.CanProceed(direction))
				{
					currentCell = currentCell.Move(direction);
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