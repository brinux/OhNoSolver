namespace brinux.ohnosolver
{
    internal class OhNoCellMoves : List<OhNoCellMove>
    {
        public int CountMoves => this.Count;
        public int CountTotalLength => this.Sum(x => x.Length);
        public OhNoDirectionEnum Direction { get; set; }

        public OhNoCellMoves(OhNoDirectionEnum direction)
        {
            Direction = direction;
        }
    }
}
