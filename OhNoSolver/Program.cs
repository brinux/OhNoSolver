namespace brinux.ohnosolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var test = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupBlockedCell(2, 2),
				OhNoCellSetup.SetupBlockedCell(4, 3),
				OhNoCellSetup.SetupFullCell(1, 1, 3),
				OhNoCellSetup.SetupFullCell(1, 4, 3),
				OhNoCellSetup.SetupFullCell(3, 1, 3),
				OhNoCellSetup.SetupFullCell(3, 3, 2),
				OhNoCellSetup.SetupFullCell(4, 1, 4),
			});

			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupBlockedCell(2, 2),
				OhNoCellSetup.SetupBlockedCell(4, 3),
				OhNoCellSetup.SetupFullCell(1, 1, 3),
				OhNoCellSetup.SetupFullCell(1, 4, 3),
				OhNoCellSetup.SetupFullCell(3, 1, 3),
				OhNoCellSetup.SetupFullCell(3, 3, 2),
				OhNoCellSetup.SetupFullCell(4, 1, 4),
			});

			OhNoSchemaPrinter.PrintSchema(schema);

			var solver = new OhNoSchemaSolver(schema);

			while (solver.Solve())
			{
				OhNoSchemaPrinter.PrintSchema(schema);
			}
		}
	}
}