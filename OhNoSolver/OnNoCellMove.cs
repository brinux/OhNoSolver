namespace brinux.ohnosolver
{
    internal class OhNoCellMove
    {
        public OhNoCellCoordinate Coordinates { get; set; }
        public int Length { get; set; }

        public OhNoCellMove(OhNoCellCoordinate coordinates, int length)
        {
            Coordinates = coordinates;
            Length = length;
        }
    }
}
