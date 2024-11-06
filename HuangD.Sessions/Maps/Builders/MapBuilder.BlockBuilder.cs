using HuangD.Sessions.Utilties;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static class BlockBuilder
    {
        public static IEnumerable<Block> Build(int high, int width, string seed)
        {
            var random = RandomBuilder.Build(seed);
            var cellRadius = 4;
            List<Index> coreIndexs = GenerateCoreIndex(high, width, random, cellRadius);

            var dict = new Dictionary<Index, Block>();
            foreach (var core in coreIndexs)
            {
                var block = new Block();
                block.Id = UUID.Generate("BLOCK");
                block.coreIndex = core;
                block.Edges = IndexMethods.GetNeighborCells(core).Values.Where(n => n.X < width && n.Y < high).ToHashSet();
                block.Indexes = block.Edges.Append(core).ToHashSet();

                foreach (var index in block.Indexes)
                {
                    dict.Add(index, block);
                }
            }

            var freeIndexes = Enumerable.Range(0, width)
                .SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y)))
                .ToHashSet();

            freeIndexes.ExceptWith(dict.Values.Distinct().SelectMany(x => x.Indexes));

            var list = dict.Values.ToHashSet();
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

            var blocks = dict.Values.Distinct().ToList();
            //var totalCellCount = blocks.Sum(x => x.Indexes.Count);

            //var smallBlocks = new Queue<Block>(blocks.Where(x => x.Indexes.Count() < totalCellCount / 150));
            //while (smallBlocks.Count != 0)
            //{
            //    var block = smallBlocks.Dequeue();

            //    var maxNeighbor = block.Neighbors.MaxBy(x => x.Indexes.Count());
            //    maxNeighbor.Indexes.UnionWith(block.Indexes);
            //    maxNeighbor.Neighbors.UnionWith(block.Neighbors.Where(x => x != maxNeighbor));
            //    maxNeighbor.Neighbors.Remove(block);

            //    maxNeighbor.Edges.UnionWith(block.Edges);
            //    maxNeighbor.Edges.RemoveWhere(x => IndexMethods.GetNeighborCells(x).All(y => maxNeighbor.Indexes.Contains(y.Value)));
            //}

            //blocks.RemoveAll(x => smallBlocks.Contains(x));
            //foreach (var block in blocks)
            //{
            //    block.Neighbors.ExceptWith(smallBlocks);
            //}

            return blocks;
        }

        private static List<Index> GenerateCoreIndex(int high, int width, System.Random random, int cellRadius)
        {
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

            return coreIndexs;
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