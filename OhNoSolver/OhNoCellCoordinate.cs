namespace brinux.ohnosolver
{
    public struct OhNoCellCoordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public OhNoCell Cell => _schema.Cells[Row][Column];

        public OhNoSchema _schema { get; private set; }

        public OhNoCellCoordinate(int row, int column, OhNoSchema schema)
        {
            Row = row;
            Column = column;
            _schema = schema;
        }

        public bool CanProceed(OhNoDirectionEnum direction, int steps = 1)
        {
            switch (direction)
            {
                case OhNoDirectionEnum.TOP:
                    return Row - steps >= 0;
                case OhNoDirectionEnum.BOTTOM:
                    return Row + steps < _schema.Height;
                case OhNoDirectionEnum.LEFT:
                    return Column - steps >= 0;
                case OhNoDirectionEnum.RIGHT:
                    return Column + steps < _schema.Width;
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }

        public OhNoCellCoordinate Move(OhNoDirectionEnum direction, int steps = 1)
        {
            switch (direction)
            {
                case OhNoDirectionEnum.TOP:
                    return new OhNoCellCoordinate(Row - steps, Column, _schema);
                case OhNoDirectionEnum.BOTTOM:
                    return new OhNoCellCoordinate(Row + steps, Column, _schema);
                case OhNoDirectionEnum.LEFT:
                    return new OhNoCellCoordinate(Row, Column - steps, _schema);
                case OhNoDirectionEnum.RIGHT:
                    return new OhNoCellCoordinate(Row, Column + steps, _schema);
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }
    }
}
