using System.Collections.Generic;
using Xunit;

namespace brinux.ohnosolver.test
{
	public class OhNoSchemaSolverTest
	{
		[Fact]
		public void UnconnectedCellAvailabilityCountTillBorder()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(3, 3, 5)
			});

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(2, 2);

			Assert.Equal(2, availability);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillBlock()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(4, 4, 5),
				OhNoCellSetup.SetupBlockedCell(4, 1)
			});

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(3, 3);

			Assert.Equal(2, availability);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillSingleFullCellWithSingleEmptyCellInBetweenNotExceedingAnyCount()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(4, 4, 5),
				OhNoCellSetup.SetupFullCell(4, 2, 5)
			});

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(3, 3);

			Assert.Equal(3, availability);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillSingleFullCellWithSingleEmptyCellInBetweenExceedingSourceCellCount()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(4, 4, 1),
				OhNoCellSetup.SetupFullCell(4, 2, 5)
			});

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(3, 3);

			Assert.Equal(0, availability);
			Assert.Equal(OhNoCellStatusEnum.Blocked, schema.Cells[3][2].Status);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillSingleFullCellWithSingleEmptyCellInBetweenExceedingTargetCellCount()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(4, 4, 5),
				OhNoCellSetup.SetupFullCell(4, 2, 1)
			});

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(3, 3);

			Assert.Equal(0, availability);
			Assert.Equal(OhNoCellStatusEnum.Blocked, schema.Cells[3][2].Status);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillMultipleFullCellsWithSingleEmptyCellInBetweenNotExceedingAnyExpectedValue()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(4, 4, 5),
				OhNoCellSetup.SetupFullCell(4, 1, 2)
			});

			schema.Cells[3][1].Status = OhNoCellStatusEnum.Full;

			OhNoSchemaPrinter.PrintSchema(schema);

			var solver = new OhNoSchemaSolver(schema);

			var availability = solver.CheckAvailabilityOnLeft(3, 3);

			Assert.Equal(0, availability);
			Assert.Equal(OhNoCellStatusEnum.Blocked, schema.Cells[3][2].Status);
		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillMultipleFullCellsWithSingleEmptyCellInBetweenExceedingSourceCellExpectedValue()
		{

		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillMultipleFullCellsWithSingleEmptyCellInBetweenExceedingTargetCellExpectedValue()
		{

		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillSingleFullCellWithMultipleEmptyCellsInBetween()
		{

		}

		[Fact]
		public void UnconnectedCellAvailabilityCountTillMultipleFullCellsWithMultipleEmptyCellsInBetween()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillBorder()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillBlock()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillSingleFullCellWithSingleEmptyCellInBetween()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillMultipleFullCellsWithSingleEmptyCellInBetweenExceedingTheExpectedValue()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillMultipleFullCellsWithSingleEmptyCellInBetweenNotExceedingTheExpectedValue()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillSingleFullCellWithMultipleEmptyCellsInBetween()
		{

		}

		[Fact]
		public void ConnectedCellAvailabilityCountTillMultipleFullCellsWithMultipleEmptyCellsInBetween()
		{

		}
	}
}