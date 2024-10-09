using DynamicData;
using HuangD.Sessions.Utilties;
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

            var waterBlocks = BuildWater(blocks, seed);
            foreach (var block in waterBlocks)
            {
                dict[block] = TerrainType.Water;
            }

            var mountionBlocks = BuildMountion(blocks.Except(waterBlocks), seed);
            foreach (var block in mountionBlocks)
            {
                dict[block] = TerrainType.Mount;
            }

            var hillBlocks = BuildHill(blocks.Except(waterBlocks).Except(mountionBlocks), mountionBlocks, seed);
            foreach (var block in hillBlocks)
            {
                dict[block] = TerrainType.Hill;
            }

            return dict;
        }

        private static IEnumerable<Block> BuildHill(IEnumerable<Block> landBlocks, IEnumerable<Block> mountionBlocks, string seed)
        {
            var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.Y));

            var maxNeighborMountCount = landBlocks.Select(x => x.Neighbors.Intersect(mountionBlocks).Count()).Max();
            var maxX = queue.Select(x => x.coreIndex.X).Max();

            var random = RandomBuilder.Build(seed);

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

        private static IEnumerable<Block> BuildMountion(IEnumerable<Block> landBlocks, string seed)
        {
            var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.X));

            var maxX = queue.Select(x => x.coreIndex.X).Max();
            var midY = queue.Select(x => x.coreIndex.Y).Average();

            var random = RandomBuilder.Build(seed);

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

        private static IEnumerable<Block> BuildWater(IEnumerable<Block> blocks, string seed)
        {
            var invalidBlocks = blocks.Where(block => block.Edges.Any(edge => edge.X == 0 || edge.Y == 0))
                        .ToHashSet();

            var maxX = blocks.SelectMany(block => block.Edges.Select(edge => edge.X)).Max();
            var maxY = blocks.SelectMany(block => block.Edges.Select(edge => edge.Y)).Max();

            var startBlocks = blocks.Where(block => block.Edges.Any(e => e.X == maxX || e.Y == maxY))
                .Except(invalidBlocks);

            var random = RandomBuilder.Build(seed);

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
    }
}