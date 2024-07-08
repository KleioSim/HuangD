namespace HuangD.Sessions.Maps;

public interface IIndexMethods
{
    Dictionary<Direction, Index> GetNeighborCells(Index index);

    Index GetNeighborCell(Index index, Direction direction);

    bool IsConnectNode(Index index, HashSet<Index> indexs);
    IEnumerable<Index> Expend(Index currentIndex, int v);
}