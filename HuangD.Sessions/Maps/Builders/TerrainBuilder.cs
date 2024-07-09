namespace HuangD.Sessions.Maps.Builders;

public static class TerrainBuilder
{
    public static Dictionary<Index, TerrainType> Build(int maxSize, string seed)
    {
        var startPoint = new Index(0, 0);

        var seaIndexs = BuildSea(maxSize);
        var landIndexs = BuildLand(startPoint, maxSize - 1, seed);
        var hillIndexs = BuildHill(startPoint, landIndexs, seed);
        var mountionIndexs = BuildMountion(startPoint, hillIndexs, seed);

        var rslt = new Dictionary<Index, TerrainType>();
        foreach (var index in seaIndexs)
        {
            rslt[index] = TerrainType.Water;
        }
        foreach (var index in landIndexs)
        {
            rslt[index] = TerrainType.Land;
        }
        foreach (var index in hillIndexs)
        {
            rslt[index] = TerrainType.Hill;
        }
        foreach (var index in mountionIndexs)
        {
            rslt[index] = TerrainType.Mount;
        }

        return rslt;
    }

    private static HashSet<Index> BuildMountion(Index startPoint, HashSet<Index> hillIndexs, string seed)
    {
        //var rslt = FlushLandEdge(hillIndexs, startPoint, seed, 0.7, false);
        //return rslt;

        var random = new Random();

        var cellQueue = new Queue<Index>(hillIndexs.OrderBy(_ => random.Next()));

        var mountionIndexs = new HashSet<Index>();
        while (cellQueue.Count != 0 && mountionIndexs.Count < hillIndexs.Count * 0.35)
        {
            var currentIndex = cellQueue.Dequeue();
            var expends = Map.IndexMethods.Expend(currentIndex, 1);

            var factor = 2 * expends.Count(x => mountionIndexs.Contains(x)) - expends.Count(x => !hillIndexs.Contains(x));

            if (random.Next(0, 100) <= factor)
            {
                mountionIndexs.Add(currentIndex);
            }
            else
            {
                cellQueue.Enqueue(currentIndex);
            }
        }

        return mountionIndexs;
    }

    private static HashSet<Index> BuildHill(Index startPoint, HashSet<Index> landIndexs, string seed)
    {

        var baseHills = AddBaseHill(landIndexs, startPoint, 1);

        var rslt = FlushLandEdge(baseHills, startPoint, seed, 0.35, true);
        rslt = FlushLandEdge(rslt, startPoint, seed, 0.15, false);

        //var isolatePlains = AddIsolatePlains(rslt, startPoint, seed, 0.25);
        //var isolateHills =  AddIsolateHill(landIndexs.Except(rslt).ToHashSet(), startPoint, seed, 0.25);

        //rslt.UnionWith(isolateHills);
        //rslt.ExceptWith(isolatePlains);
        return rslt;
    }

    private static HashSet<Index> AddBaseHill(HashSet<Index> landIndexs, Index startPoint, double percent)
    {
        var maxX = landIndexs.Select(index => index.X).Max();
        var maxY = landIndexs.Select(index => index.Y).Max();

        var baseLength = Math.Max(maxX, maxY);

        var rslt = new HashSet<Index>();
        for (int i = 0; i < baseLength; i++)
        {
            for (int j = 0; j < baseLength; j++)
            {
                var index = new Index(Math.Abs(startPoint.X - i), Math.Abs(startPoint.Y - j));
                if (landIndexs.Contains(index))
                {
                    rslt.Add(index);
                    if (rslt.Count() > landIndexs.Count() * percent)
                    {
                        return rslt;
                    }
                }

                index = new Index(Math.Abs(startPoint.X - j), Math.Abs(startPoint.Y - i));
                if (landIndexs.Contains(index))
                {

                    rslt.Add(index);
                    if (rslt.Count() > landIndexs.Count() * percent)
                    {
                        return rslt;
                    }
                }

            }
        }

        return rslt;
    }

    private static HashSet<Index> AddIsolateHill(HashSet<Index> indexs, Index startPoint, string seed, double percent)
    {
        var random = new Random();

        var cellQueue = new Queue<Index>(indexs.OrderBy(_ => random.Next()));

        var eraserIndexs = new HashSet<Index>();
        while (eraserIndexs.Count < indexs.Count * 0.35)
        {
            var currentIndex = cellQueue.Dequeue();
            var expends = Map.IndexMethods.Expend(currentIndex, 3);

            if (random.Next(0, 100) <= expends.Count(e => eraserIndexs.Contains(e)) * 100.0 / expends.Count())
            {
                eraserIndexs.Add(currentIndex);
            }
            else
            {
                cellQueue.Enqueue(currentIndex);
            }
        }

        return eraserIndexs;
    }

    private static HashSet<Index> AddIsolatePlains(HashSet<Index> indexs, Index startPoint, string seed, double percent)
    {

        var random = new Random();

        var cellQueue = new Queue<Index>(indexs.OrderBy(_ => random.Next()));

        var eraserIndexs = new HashSet<Index>();
        while (eraserIndexs.Count < indexs.Count * 0.25)
        {
            var currentIndex = cellQueue.Dequeue();
            var expends = Map.IndexMethods.Expend(currentIndex, 3);

            if (random.Next(0, 100) <= expends.Count(e => eraserIndexs.Contains(e)) * 100.0 / expends.Count())
            {
                eraserIndexs.Add(currentIndex);
            }
            else
            {
                cellQueue.Enqueue(currentIndex);
            }
        }

        return eraserIndexs;
    }

    private static HashSet<Index> BuildSea(int maxSize)
    {
        return Enumerable.Range(0, maxSize).SelectMany(x => Enumerable.Range(0, maxSize).Select(y => new Index(x, y))).ToHashSet();
    }

    private static HashSet<Index> BuildLand(Index startPoint, int landSize, string seed)
    {

        var rslt = new HashSet<Index>();
        for (int i = 0; i < landSize; i++)
        {
            for (int j = 0; j < landSize; j++)
            {
                var index = new Index(Math.Abs(startPoint.X - i), Math.Abs(startPoint.Y - j));
                rslt.Add(index);
            }
        }

        return FlushLandEdge(rslt, startPoint, seed, 0.25, false);
    }

    private static HashSet<Index> FlushLandEdge(HashSet<Index> indexs, Index startPoint, string seed, double percent, bool allowIsland)
    {
        var random = new Random();
        Dictionary<Index, int> edgeFactors = indexs.Where(index =>
        {
            if (startPoint.X == index.X || startPoint.Y == index.Y)
            {
                return false;
            }

            var neighbors = Map.IndexMethods.GetNeighborCells(index).Values;
            if (neighbors.All(x => indexs.Contains(x)))
            {
                return false;
            }

            return true;

        }).ToDictionary(k => k, v => 1); ;

        var rslt = new HashSet<Index>(indexs);

        int eraseCount = 0;
        var gCount = rslt.Count();
        while (true)
        {
            var eraserIndexs = new List<Index>();

            foreach (var index in edgeFactors.Keys.OrderBy(x => x.ToString()).ToArray())
            {
                var factor = edgeFactors[index];

                if (!allowIsland)
                {
                    if (Map.IndexMethods.IsConnectNode(index, rslt))
                    {
                        edgeFactors.Remove(index);
                        continue;
                    }
                }

                var factor2 = Map.IndexMethods.GetNeighborCells(index).Values.Where(x => rslt.Contains(x)).Count();
                if (!allowIsland && factor2 <= 3 || random.Next(0, 10000) <= 1800 / factor)
                {
                    edgeFactors.Remove(index);
                    rslt.Remove(index);
                    eraseCount++;
                    eraserIndexs.Add(index);

                    if (eraseCount * 1.0 / gCount > percent)
                    {
                        return rslt;
                    }
                }
                else
                {
                    edgeFactors[index] += 3;
                }

            }

            foreach (var index in eraserIndexs)
            {
                var neighbors = Map.IndexMethods.GetNeighborCells(index).Values.Where(x => rslt.Contains(x));
                foreach (var neighbor in neighbors)
                {
                    edgeFactors.TryAdd(neighbor, 1);
                }
            }

        }
    }
}