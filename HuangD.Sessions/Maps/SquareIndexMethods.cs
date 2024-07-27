namespace HuangD.Sessions.Maps;

public class SquareIndexMethods : IIndexMethods
{
    Dictionary<Direction, (int x, int y)> Direction2Index = new Dictionary<Direction, (int x, int y)>()
    {
        { Direction.TopSide, (0,-1) },
        { Direction.TopLeftCorner, (-1, -1) },
        { Direction.LeftSide,(-1, 0) },
        { Direction.BottomLeftCorner,(-1, 1)},
        { Direction.BottomSide,(0, 1)},
        { Direction.BottomRightCorner,(1, 1)},
        { Direction.RightSide,(1, 0)},
        { Direction.TopRightCorner,(1, -1)}
    };
    (int x, int y)[] Neighbors = new (int x, int y)[]
    {
        (1,0),
        (0,1),
        (-1,0),
        (0,-1),
        (1,-1),
        (-1,1),
        (-1,-1),
        (1,1)
    };

    public IEnumerable<Index> Expend(Index index, int Length)
    {
        for (int i = -Length; i <= Length; i++)
        {
            for (int j = -Length; j <= Length; j++)
            {
                if (i == 0 && j == 0) continue;
                yield return new Index(index.X - i, index.Y - j);
            }
        }
    }

    public Index GetNeighborCell(Index index, Direction direction)
    {
        return new Index(index.X + Direction2Index[direction].x, index.Y + Direction2Index[direction].y);
    }


    public Dictionary<Direction, Index> GetNeighborCells(Index index)
    {
        return Direction2Index.ToDictionary(n => n.Key, m => new Index(index.X + m.Value.x, index.Y + m.Value.y));
    }

    public Dictionary<Direction, Index> GetNeighborCells4(Index index)
    {
        var directs = new HashSet<Direction>()
        {
            Direction.TopSide,
            Direction.BottomSide,
            Direction.LeftSide,
            Direction.RightSide
        };
        
        return directs.ToDictionary(n => n, m => new Index(index.X + Direction2Index[m].x, index.Y + Direction2Index[m].y));
    }

    public bool IsConnectNode(Index index, HashSet<Index> indexs)
    {
        var neighbors = GetNeighborCells(index);

        if (indexs.Contains(neighbors[Direction.LeftSide]) && indexs.Contains(neighbors[Direction.RightSide])
            && !indexs.Contains(neighbors[Direction.BottomSide]) && !indexs.Contains(neighbors[Direction.TopSide]))
        {
            return true;
        }
        if (!indexs.Contains(neighbors[Direction.LeftSide]) && !indexs.Contains(neighbors[Direction.RightSide])
            && indexs.Contains(neighbors[Direction.BottomSide]) && indexs.Contains(neighbors[Direction.TopSide]))
        {
            return true;
        }
        if (indexs.Contains(neighbors[Direction.LeftSide]) && indexs.Contains(neighbors[Direction.BottomSide])
            && !indexs.Contains(neighbors[Direction.BottomLeftCorner]))
        {
            return true;
        }
        if (indexs.Contains(neighbors[Direction.LeftSide]) && indexs.Contains(neighbors[Direction.TopSide])
            && !indexs.Contains(neighbors[Direction.TopLeftCorner]))
        {
            return true;
        }
        if (indexs.Contains(neighbors[Direction.RightSide]) && indexs.Contains(neighbors[Direction.BottomSide])
            && !indexs.Contains(neighbors[Direction.BottomRightCorner]))
        {
            return true;
        }
        if (indexs.Contains(neighbors[Direction.RightSide]) && indexs.Contains(neighbors[Direction.TopSide])
            && !indexs.Contains(neighbors[Direction.TopRightCorner]))
        {
            return true;
        }

        return false;
    }
}
