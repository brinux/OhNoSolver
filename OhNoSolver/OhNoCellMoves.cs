namespace brinux.ohnosolver
{
    internal class OhNoCellMoves : List<OhNoCellMove>
    {
        public int CountMoves => this.Count;
        public int CountTotalLength => this.Sum(x => x.Length);
    }
}
