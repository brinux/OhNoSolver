namespace brinux.ohnosolver
{
	public class OhNoSchemaPrinter
	{
		public static void PrintSchema(OhNoSchema schema)
		{
			if (schema == null)
			{
				throw new NullReferenceException("The schema is undefined");
			}

			Console.Write("   ");
			for (int c = 0; c < schema.Width; c++)
			{
				Console.Write($" { c + 1 }{(c < 10 ? " " : "")}");
			}
			Console.WriteLine();

			for (int r = 0; r < schema.Height; r++)
			{
				Console.Write($" { r + 1 }{(r + 1 < 10 ? " " : "")}");

				for (int c = 0; c < schema.Width; c++)
				{
					switch (schema.Cells[r][c].Status)
					{
						case OhNoCellStatusEnum.Full:
							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.DarkBlue;
							Console.Write($"{(schema.Cells[r][c].HasValue ? (schema.Cells[r][c].Value >= 10 ? schema.Cells[r][c].Value : (" " + schema.Cells[r][c].Value)) : "  ")} ");
							break;

						case OhNoCellStatusEnum.Blocked:
							Console.BackgroundColor = ConsoleColor.Red;
							Console.Write("   ");
							break;

						default:
							Console.BackgroundColor = ConsoleColor.Black;
							Console.Write("   ");
							break;
					}
				}

				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Gray;

				Console.WriteLine();
			}

			Console.WriteLine();

			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}
