namespace brinux.ohnosolver
{
    internal class OhNoCellMove
    {
        public OhNoCellCoordinate Cell { get; set; }
        public int Length { get; set; }

        public OhNoCellMove(OhNoCellCoordinate cell, int length)
        {
            Cell = cell;
            Length = length;
        }
    }
}
