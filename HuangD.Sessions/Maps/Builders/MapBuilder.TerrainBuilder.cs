using DynamicData;
using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static class TerrainBuilder
    {
        public static Dictionary<Block, TerrainType> Build(IEnumerable<Block> blocks, string seed)
        {
            var dict = blocks.ToDictionary(x => x, _ => TerrainType.Land);
            var random = RandomBuilder.Build(seed);

            var waterBlocks = BuildWater(blocks, random);
            foreach (var block in waterBlocks)
            {
                dict[block] = TerrainType.Water;
            }

            var mountionBlocks = BuildMountion(blocks.Except(waterBlocks), random);
            foreach (var block in mountionBlocks)
            {
                dict[block] = TerrainType.Mount;
            }

            var hillBlocks = BuildHill(blocks.Except(waterBlocks).Except(mountionBlocks), mountionBlocks, random);
            foreach (var block in hillBlocks)
            {
                dict[block] = TerrainType.Hill;
            }

            return dict;
        }

        private static IEnumerable<Block> BuildHill(IEnumerable<Block> landBlocks, IEnumerable<Block> mountionBlocks, Random random)
        {
            var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.Y));

            var maxNeighborMountCount = landBlocks.Select(x => x.Neighbors.Intersect(mountionBlocks).Count()).Max();
            var maxX = queue.Select(x => x.coreIndex.X).Max();

            while (queue.Count >= landBlocks.Count() * 0.4)
            {
                var maxNeighborCount = queue.Select(x => x.Neighbors.Intersect(queue).Count()).Max();

                var curr = queue.Dequeue();

                var factor1 = random.Next(0, 300);
                //var factor2 = 0;
                var factor2 = curr.coreIndex.X * 400.0 / maxX;
                //var factor3 = 0;
                var factor3 = (maxNeighborMountCount - curr.Neighbors.Intersect(mountionBlocks).Count()) * 300 / maxNeighborMountCount;

                if (random.Next(0, 1001) < factor1 + factor2 + factor3)
                {
                    continue;
                }

                queue.Enqueue(curr);

            }

            return queue;
        }

        private static IEnumerable<Block> BuildMountion(IEnumerable<Block> landBlocks, Random random)
        {
            var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.X));

            var maxX = queue.Select(x => x.coreIndex.X).Max();
            var midY = queue.Select(x => x.coreIndex.Y).Average();

            while (queue.Count >= landBlocks.Count() * 0.3)
            {
                var maxNeighborCount = queue.Select(x => x.Neighbors.Intersect(queue).Count()).Max();

                var curr = queue.Dequeue();

                //var factor1 = 0;
                var factor1 = curr.Neighbors.Intersect(queue).Count() * 300.0 / maxNeighborCount;

                //var factor2 = 700;
                var factor2 = curr.coreIndex.X * 700.0 / maxX;

                //var factor3 = int.MaxValue;
                var factor3 = (midY - System.Math.Abs(curr.coreIndex.Y - midY)) * 700.0 / midY;

                var factor = (factor1 + System.Math.Min(factor2, factor3));
                if (random.Next(0, 1001) < factor)
                {
                    continue;
                }

                queue.Enqueue(curr);

            }

            return queue;
        }

        private static IEnumerable<Block> BuildWater(IEnumerable<Block> blocks, Random random)
        {
            var invalidBlocks = blocks.Where(block => block.Edges.Any(edge => edge.X == 0 || edge.Y == 0))
                        .ToHashSet();

            var maxX = blocks.SelectMany(block => block.Edges.Select(edge => edge.X)).Max();
            var maxY = blocks.SelectMany(block => block.Edges.Select(edge => edge.Y)).Max();

            var startBlocks = blocks.Where(block => block.Edges.Any(e => e.X == maxX || e.Y == maxY))
                .Except(invalidBlocks);

            var rslt = new HashSet<Block>(startBlocks);
            var queue = new Queue<Block>(startBlocks.OrderBy(_ => random.Next()));

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();
                int count = random.Next(1, 3);
                while (true)
                {
                    var next = current.Neighbors.Where(n => !rslt.Contains(n) && n.coreIndex.X <= current.coreIndex.X && n.coreIndex.Y <= current.coreIndex.Y)
                        .FirstOrDefault();
                    if (next == null)
                    {
                        break;
                    }

                    current = next;

                    rslt.Add(current);
                    if (rslt.Count > blocks.Count() * 0.2)
                    {
                        goto Finish;
                    }

                    count--;
                    if (count == 0)
                    {
                        break;
                    }

                }
            }
        Finish:
            return rslt;
        }

        public static Dictionary<Index, TerrainType> Build(int maxSize, string seed)
        {
            //var random = new Random();

            //var rslt = new Dictionary<Index, TerrainType>();
            //foreach (var index in Enumerable.Range(0, maxSize).SelectMany(x => Enumerable.Range(0, maxSize).Select(y => new Index(x, y))))
            // {
            //    rslt.Add(index, random.Next(0, 100) > 50 ? TerrainType.Land : TerrainType.Water);
            // }

            //return rslt;
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

            var rslt = FlushLandEdge(hillIndexs, startPoint, seed, 0.65, false);

            var random = RandomBuilder.Build(seed);

            for (int i = 0; i < 10; i++)
            {
                var needRemoves = new HashSet<Index>();
                foreach (var index in rslt)
                {
                    var neighorCount = MapCell.IndexMethods.GetNeighborCells(index).Values.Count(x => rslt.Contains(x));
                    if (random.Next(0, 100) < 2)
                    {
                        needRemoves.Add(index);
                    }
                }

                rslt = rslt.Except(needRemoves).ToHashSet();
            }


            return rslt;
        }

        private static HashSet<Index> BuildHill(Index startPoint, HashSet<Index> landIndexs, string seed)
        {
            var baseHills = AddBaseHill(landIndexs, startPoint, 1, seed);

            var rslt = FlushWithAutoCell(baseHills, landIndexs, seed);
            return rslt;
        }

        private static HashSet<Index> FlushWithAutoCell(HashSet<Index> hillIndex, HashSet<Index> landIndex, string seed)
        {
            var random = RandomBuilder.Build(seed);

            var needRemoved = new HashSet<Index>();
            var needAdded = new HashSet<Index>();

            for (int i = 0; i < 3; i++)
            {
                foreach (var index in hillIndex)
                {
                    var count = MapCell.IndexMethods.GetNeighborCells(index).Values.Count(x => hillIndex.Contains(x));
                    if (count < 3)
                    {
                        needRemoved.Add(index);
                    }
                }

                foreach (var index in landIndex.Except(hillIndex))
                {
                    var count = MapCell.IndexMethods.GetNeighborCells(index).Values.Count(x => hillIndex.Contains(x));
                    if (count > 6)
                    {
                        needAdded.Add(index);
                    }
                }

                hillIndex = hillIndex.Except(needRemoved).Union(needAdded).ToHashSet();
            }


            return hillIndex;

        }

        private static HashSet<Index> AddBaseHill(HashSet<Index> landIndexs, Index startPoint, double percent, string seed)
        {
            var random = RandomBuilder.Build(seed);
            var dict = landIndexs.ToDictionary(k => k, v =>
            {
                var xdisct = Math.Abs(v.X - startPoint.X);
                var ydisct = Math.Abs(v.Y - startPoint.Y);

                var maxYDist = landIndexs.Max(i => i.Y);
                if (ydisct < landIndexs.Max(i => i.Y) * 0.15)
                {
                    ydisct = (int)(maxYDist * random.Next(50, 100) / 100.0);
                }

                return xdisct - ydisct;
            });
            dict = dict.ToDictionary(k => k.Key, v => v.Value - dict.Values.Min());


            return dict.Where(p => p.Key.X == startPoint.X || p.Key.Y == startPoint.Y || (random.Next(1, 101) > p.Value * 70 / dict.Values.Max())).Select(p => p.Key).ToHashSet();

            //var maxX = landIndexs.Select(index => index.X).Max();
            //var maxY = landIndexs.Select(index => index.Y).Max();

            //var baseLength = Math.Max(maxX, maxY);

            //var rslt = new HashSet<Index>();
            //for (int i = 0; i < baseLength; i++)
            //{
            //    for (int j = 0; j < baseLength; j++)
            //    {
            //        var index = new Index(Math.Abs(startPoint.X - i), Math.Abs(startPoint.Y - j));
            //        if (landIndexs.Contains(index))
            //        {
            //            rslt.Add(index);
            //            if (rslt.Count() > landIndexs.Count() * percent)
            //            {
            //                return rslt;
            //            }
            //        }

            //        index = new Index(Math.Abs(startPoint.X - j), Math.Abs(startPoint.Y - i));
            //        if (landIndexs.Contains(index))
            //        {

            //            rslt.Add(index);
            //            if (rslt.Count() > landIndexs.Count() * percent)
            //            {
            //                return rslt;
            //            }
            //        }

            //    }
            //}

            //return rslt;
        }

        //private static HashSet<Index> AddIsolateHill(HashSet<Index> indexs, Index startPoint, string seed, double percent)
        //{
        //    var random = new Random();

        //    var cellQueue = new Queue<Index>(indexs.OrderBy(_ => random.Next()));

        //    var eraserIndexs = new HashSet<Index>();
        //    while (eraserIndexs.Count < indexs.Count * 0.35)
        //    {
        //        var currentIndex = cellQueue.Dequeue();
        //        var expends = MapCell.IndexMethods.Expend(currentIndex, 3);

        //        if (random.Next(0, 100) <= expends.Count(e => eraserIndexs.Contains(e)) * 100.0 / expends.Count())
        //        {
        //            eraserIndexs.Add(currentIndex);
        //        }
        //        else
        //        {
        //            cellQueue.Enqueue(currentIndex);
        //        }
        //    }

        //    return eraserIndexs;
        //}

        //private static HashSet<Index> AddIsolatePlains(HashSet<Index> indexs, Index startPoint, string seed, double percent)
        //{

        //    var random = new Random();

        //    var cellQueue = new Queue<Index>(indexs.OrderBy(_ => random.Next()));

        //    var eraserIndexs = new HashSet<Index>();
        //    while (eraserIndexs.Count < indexs.Count * 0.25)
        //    {
        //        var currentIndex = cellQueue.Dequeue();
        //        var expends = MapCell.IndexMethods.Expend(currentIndex, 3);

        //        if (random.Next(0, 100) <= expends.Count(e => eraserIndexs.Contains(e)) * 100.0 / expends.Count())
        //        {
        //            eraserIndexs.Add(currentIndex);
        //        }
        //        else
        //        {
        //            cellQueue.Enqueue(currentIndex);
        //        }
        //    }

        //    return eraserIndexs;
        //}

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
            var random = RandomBuilder.Build(seed);
            Dictionary<Index, int> edgeFactors = indexs.Where(index =>
            {
                if (startPoint.X == index.X || startPoint.Y == index.Y)
                {
                    return false;
                }

                var neighbors = MapCell.IndexMethods.GetNeighborCells(index).Values;
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
                        if (MapCell.IndexMethods.IsConnectNode(index, rslt))
                        {
                            edgeFactors.Remove(index);
                            continue;
                        }
                    }

                    var factor2 = MapCell.IndexMethods.GetNeighborCells(index).Values.Where(x => rslt.Contains(x)).Count();
                    if ((!allowIsland && factor2 <= 3) || random.Next(0, 3000) <= 1000 / factor)
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
                        edgeFactors[index] += 1 + edgeFactors[index] / 3;
                    }

                }

                foreach (var index in eraserIndexs)
                {
                    var neighbors = MapCell.IndexMethods.GetNeighborCells(index).Values.Where(x => rslt.Contains(x));
                    foreach (var neighbor in neighbors)
                    {
                        edgeFactors.TryAdd(neighbor, 1);
                    }
                }

            }
        }
    }
}