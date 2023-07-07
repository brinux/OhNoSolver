namespace brinux.ohnosolver
{
    internal struct OhNoCellCoordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public OhNoCellCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
