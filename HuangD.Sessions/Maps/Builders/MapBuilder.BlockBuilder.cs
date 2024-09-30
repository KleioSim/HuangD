using HuangD.Sessions.Utilties;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static class BlockBuilder
    {
        internal static IEnumerable<Block> Build(int high, int width, string seed)
        {
            var random = RandomBuilder.Build(seed);

            var dict = new Dictionary<Index, Block>();
            var list = new HashSet<Block>();

            var cellRadius = 4;

            var coreIndexs = new List<Index>();

            var fullIndexes = new Queue<Index>(Enumerable.Range(cellRadius, width - cellRadius)
                .SelectMany(x => Enumerable.Range(cellRadius, high - cellRadius).Select(y => new Index(x, y)))
                .OrderBy(_ => random.Next()));

            while (fullIndexes.Count != 0)
            {
                var curr = fullIndexes.Dequeue();
                if (coreIndexs.Any(index => System.Math.Abs(index.X - curr.X) < cellRadius * 2 && System.Math.Abs(index.Y - curr.Y) < cellRadius * 2))
                {
                    continue;
                }

                coreIndexs.Add(curr);
            }

            foreach (var core in coreIndexs)
            {
                var block = new Block();
                block.coreIndex = core;
                block.Edges = IndexMethods.GetNeighborCells(core).Values.Where(n => n.X < width && n.Y < high).ToList();
                block.Indexes = block.Edges.Append(core).ToList();

                list.Add(block);

                foreach (var index in block.Indexes)
                {
                    dict.Add(index, block);
                }
            }

            var freeIndexes = Enumerable.Range(0, width)
                .SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y)))
                .ToHashSet();

            freeIndexes.ExceptWith(dict.Values.Distinct().SelectMany(x => x.Indexes));

            var finishedBlocks = new HashSet<Block>();

            while (freeIndexes.Count != 0 && finishedBlocks.Count < list.Count)
            {
                foreach (var block in list.Except(finishedBlocks))
                {

                    var validEgdes = block.Edges.Except(block.InvaildEdges);
                    while (validEgdes.Count() != 0)
                    {
                        var edge = validEgdes.ElementAt(random.Next(0, validEgdes.Count()));

                        var neighborIndexes = IndexMethods.GetNeighborCells4(edge).Values;
                        var newEdges = neighborIndexes
                            .Where(x => freeIndexes.Contains(x))
                            .ToArray();
                        if (newEdges.Length == 0)
                        {
                            if (neighborIndexes.All(index => block.Indexes.Contains(index)))
                            {
                                block.Edges.Remove(edge);
                            }
                            else
                            {
                                block.InvaildEdges.Add(edge);
                            }
                        }
                        else
                        {
                            var newEdge = newEdges.ElementAt(random.Next(0, newEdges.Count()));

                            block.Edges.Add(newEdge);
                            block.Indexes.Add(newEdge);

                            freeIndexes.Remove(newEdge);

                            dict.Add(newEdge, block);

                            var neighborBlocks = GetNeighborBlock(newEdge, dict);
                            block.Neighbors.Union(neighborBlocks);
                            foreach (var neighborBlock in neighborBlocks)
                            {
                                neighborBlock.Neighbors.Add(block);
                            }

                            break;
                        }
                    }

                    if (validEgdes.Count() == 0)
                    {
                        finishedBlocks.Add(block);
                    }
                }
            }

            return dict.Values.Distinct();
        }

        private static IEnumerable<Block> GetNeighborBlock(Index newEdge, Dictionary<Index, Block> dict)
        {
            var rslt = new HashSet<Block>();
            foreach (var index in IndexMethods.GetNeighborCells4(newEdge).Values)
            {
                if (dict.TryGetValue(index, out var block))
                {
                    rslt.Add(block);
                }
            }
            return rslt;
        }
    }
}