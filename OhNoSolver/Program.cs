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

			var test2 = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupBlockedCell(2, 2),
				OhNoCellSetup.SetupBlockedCell(4, 3),
				OhNoCellSetup.SetupFullCell(1, 1, 3),
				OhNoCellSetup.SetupFullCell(1, 4, 3),
				OhNoCellSetup.SetupFullCell(3, 1, 3),
				OhNoCellSetup.SetupFullCell(3, 3, 2),
				OhNoCellSetup.SetupFullCell(4, 1, 4),
			});

            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupBlockedCell(2, 7),
                OhNoCellSetup.SetupBlockedCell(2, 9),
                OhNoCellSetup.SetupBlockedCell(3, 6),
                OhNoCellSetup.SetupBlockedCell(5, 1),
                OhNoCellSetup.SetupBlockedCell(7, 8),
                OhNoCellSetup.SetupBlockedCell(8, 6),
                OhNoCellSetup.SetupBlockedCell(9, 6),

                OhNoCellSetup.SetupFullCell(1, 1, 1),
                OhNoCellSetup.SetupFullCell(1, 4, 7),
                OhNoCellSetup.SetupFullCell(1, 6, 6),
                OhNoCellSetup.SetupFullCell(1, 8, 8),

                OhNoCellSetup.SetupFullCell(2, 2, 1),
                OhNoCellSetup.SetupFullCell(2, 4, 1),

                OhNoCellSetup.SetupFullCell(3, 8, 2),

                OhNoCellSetup.SetupFullCell(4, 2, 3),
				OhNoCellSetup.SetupFullCell(4, 6, 2),
                
				OhNoCellSetup.SetupFullCell(5, 2, 5),
				OhNoCellSetup.SetupFullCell(5, 3, 6),
				OhNoCellSetup.SetupFullCell(5, 4, 5),
				OhNoCellSetup.SetupFullCell(5, 9, 4),

                OhNoCellSetup.SetupFullCell(6, 4, 2),
				OhNoCellSetup.SetupFullCell(6, 5, 5),
				OhNoCellSetup.SetupFullCell(6, 7, 2),

				OhNoCellSetup.SetupFullCell(7, 1, 3),
				OhNoCellSetup.SetupFullCell(7, 3, 2),
                OhNoCellSetup.SetupFullCell(7, 6, 1),

                OhNoCellSetup.SetupFullCell(8, 5, 8),
                OhNoCellSetup.SetupFullCell(8, 8, 2),

                OhNoCellSetup.SetupFullCell(9, 2, 5),
                OhNoCellSetup.SetupFullCell(9, 9, 2)
            });

            OhNoSchemaPrinter.PrintSchema(schema);

			var solver = new OhNoSchemaSolver(schema);

			while (solver.Solve())
			{
                OhNoSchemaPrinter.PrintSchema(schema);
             
				//Console.ReadKey();
			}

			if (schema.IsCompleted())
			{
				Console.WriteLine("Schema correctly completed");
			}
			else
			{
                Console.WriteLine("It was not possible to complete the schema");
			}
		}
	}
}