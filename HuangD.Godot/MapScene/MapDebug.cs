using Godot;
using HuangD.Sessions.Maps;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public partial class MapDebug : Node2D
{
    BlockMap BlockMap => GetNode<BlockMap>("BlockMap");
    TerrainMap TerrainMap => GetNode<TerrainMap>("TerrainMap");

    //IIndexMethods IndexMethods = new SquareIndexMethods();

    public override void _Ready()
    {
        //var blocks = BuildBlocks(120, 120);

        //var random = new System.Random();

        //for (int i = 0; i < blocks.Count(); i++)
        //{
        //    var color = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);
        //    TileMap.AddLayer(i);
        //    TileMap.SetLayerModulate(i, color);

        //    foreach (var index in blocks.ElementAt(i).Indexes)
        //    {
        //        TileMap.SetCell(i, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        //    }
        //}

        //var block2Terrain = BuildTerrains(blocks);
        //foreach (var pair in block2Terrain)
        //{
        //    foreach (var index in pair.Key.Indexes)
        //    {
        //        TerrainMap.AddOrUpdate(index, pair.Value);
        //    }
        //}
    }

    //private Dictionary<Block, TerrainType> BuildTerrains(IEnumerable<Block> blocks)
    //{
    //    var dict = blocks.ToDictionary(x => x, _ => TerrainType.Land);

    //    var waterBlocks = BuildWater(blocks);
    //    foreach (var block in waterBlocks)
    //    {
    //        dict[block] = TerrainType.Water;
    //    }

    //    var mountionBlocks = BuildMountion(blocks.Except(waterBlocks));
    //    foreach (var block in mountionBlocks)
    //    {
    //        dict[block] = TerrainType.Mount;
    //    }

    //    var hillBlocks = BuildHill(blocks.Except(waterBlocks).Except(mountionBlocks), mountionBlocks);
    //    foreach (var block in hillBlocks)
    //    {
    //        dict[block] = TerrainType.Hill;
    //    }

    //    return dict;
    //}

    //private IEnumerable<Block> BuildHill(IEnumerable<Block> landBlocks, IEnumerable<Block> mountionBlocks)
    //{
    //    var random = new System.Random();
    //    var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.Y));

    //    var maxNeighborMountCount = landBlocks.Select(x => x.Neighbors.Intersect(mountionBlocks).Count()).Max();
    //    var maxX = queue.Select(x => x.coreIndex.X).Max();

    //    while (queue.Count >= landBlocks.Count() * 0.4)
    //    {
    //        var maxNeighborCount = queue.Select(x => x.Neighbors.Intersect(queue).Count()).Max();

    //        var curr = queue.Dequeue();

    //        var factor1 = random.Next(0, 300);
    //        //var factor2 = 0;
    //        var factor2 = curr.coreIndex.X * 400.0 / maxX;
    //        //var factor3 = 0;
    //        var factor3 = (maxNeighborMountCount - curr.Neighbors.Intersect(mountionBlocks).Count()) * 300 / maxNeighborMountCount;

    //        if (random.Next(0, 1001) < factor1 + factor2 + factor3)
    //        {
    //            continue;
    //        }

    //        queue.Enqueue(curr);

    //    }

    //    return queue;
    //}

    //private IEnumerable<Block> BuildMountion(IEnumerable<Block> landBlocks)
    //{
    //    var random = new System.Random();
    //    var queue = new Queue<Block>(landBlocks.OrderByDescending(block => block.coreIndex.X));

    //    var maxX = queue.Select(x => x.coreIndex.X).Max();
    //    var midY = queue.Select(x => x.coreIndex.Y).Average();

    //    while (queue.Count >= landBlocks.Count() * 0.3)
    //    {
    //        var maxNeighborCount = queue.Select(x => x.Neighbors.Intersect(queue).Count()).Max();

    //        var curr = queue.Dequeue();

    //        //var factor1 = 0;
    //        var factor1 = curr.Neighbors.Intersect(queue).Count() * 300.0 / maxNeighborCount;

    //        //var factor2 = 700;
    //        var factor2 = curr.coreIndex.X * 700.0 / maxX;

    //        //var factor3 = int.MaxValue;
    //        var factor3 = (midY - System.Math.Abs(curr.coreIndex.Y - midY)) * 700.0 / midY;

    //        var factor = (factor1 + System.Math.Min(factor2, factor3));
    //        if (random.Next(0, 1001) < factor)
    //        {
    //            continue;
    //        }

    //        queue.Enqueue(curr);

    //    }

    //    return queue;
    //}

    //private IEnumerable<Block> BuildWater(IEnumerable<Block> blocks)
    //{
    //    var invalidBlocks = blocks.Where(block => block.Edges.Any(edge => edge.X == 0 || edge.Y == 0))
    //                .ToHashSet();

    //    var maxX = blocks.SelectMany(block => block.Edges.Select(edge => edge.X)).Max();
    //    var maxY = blocks.SelectMany(block => block.Edges.Select(edge => edge.Y)).Max();

    //    var random = new System.Random();
    //    var startBlocks = blocks.Where(block => block.Edges.Any(e => e.X == maxX || e.Y == maxY))
    //        .Except(invalidBlocks);

    //    var rslt = new HashSet<Block>(startBlocks);
    //    var queue = new Queue<Block>(startBlocks.OrderBy(_ => random.Next()));

    //    while (queue.Count != 0)
    //    {
    //        var current = queue.Dequeue();
    //        int count = random.Next(1, 3);
    //        while (true)
    //        {
    //            var next = current.Neighbors.Where(n => !rslt.Contains(n) && n.coreIndex.X <= current.coreIndex.X && n.coreIndex.Y <= current.coreIndex.Y)
    //                .FirstOrDefault();
    //            if (next == null)
    //            {
    //                break;
    //            }

    //            current = next;

    //            rslt.Add(current);
    //            if (rslt.Count > blocks.Count() * 0.2)
    //            {
    //                goto Finish;
    //            }

    //            count--;
    //            if (count == 0)
    //            {
    //                break;
    //            }

    //        }
    //    }
    //Finish:
    //    return rslt;
    //}

    //private IEnumerable<Block> BuildBlocks(int width, int high)
    //{
    //    var random = new System.Random();

    //    var dict = new Dictionary<Index, Block>();
    //    var list = new HashSet<Block>();

    //    var cellRadius = 4;

    //    var coreIndexs = new List<Index>();

    //    var fullIndexes = new Queue<Index>(Enumerable.Range(cellRadius, width - cellRadius)
    //        .SelectMany(x => Enumerable.Range(cellRadius, high - cellRadius).Select(y => new Index(x, y)))
    //        .OrderBy(_ => random.Next()));

    //    while (fullIndexes.Count != 0)
    //    {
    //        var curr = fullIndexes.Dequeue();
    //        if (coreIndexs.Any(index => System.Math.Abs(index.X - curr.X) < cellRadius * 2 && System.Math.Abs(index.Y - curr.Y) < cellRadius * 2))
    //        {
    //            continue;
    //        }

    //        coreIndexs.Add(curr);
    //    }

    //    foreach (var core in coreIndexs)
    //    {
    //        var block = new Block();
    //        block.coreIndex = core;
    //        block.Edges = IndexMethods.GetNeighborCells(core).Values.Where(n => n.X < width && n.Y < high).ToList();
    //        block.Indexes = block.Edges.Append(core).ToList();

    //        list.Add(block);

    //        foreach (var index in block.Indexes)
    //        {
    //            dict.Add(index, block);
    //        }
    //    }

    //    var freeIndexes = Enumerable.Range(0, width)
    //        .SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y)))
    //        .ToHashSet();

    //    freeIndexes.ExceptWith(dict.Values.Distinct().SelectMany(x => x.Indexes));

    //    var finishedBlocks = new HashSet<Block>();

    //    while (freeIndexes.Count != 0 && finishedBlocks.Count < list.Count)
    //    {
    //        foreach (var block in list.Except(finishedBlocks))
    //        {

    //            var validEgdes = block.Edges.Except(block.InvaildEdges);
    //            while (validEgdes.Count() != 0)
    //            {
    //                var edge = validEgdes.ElementAt(random.Next(0, validEgdes.Count()));

    //                var neighborIndexes = IndexMethods.GetNeighborCells4(edge).Values;
    //                var newEdges = neighborIndexes
    //                    .Where(x => freeIndexes.Contains(x))
    //                    .ToArray();
    //                if (newEdges.Length == 0)
    //                {
    //                    if (neighborIndexes.All(index => block.Indexes.Contains(index)))
    //                    {
    //                        block.Edges.Remove(edge);
    //                    }
    //                    else
    //                    {
    //                        block.InvaildEdges.Add(edge);
    //                    }
    //                }
    //                else
    //                {
    //                    var newEdge = newEdges.ElementAt(random.Next(0, newEdges.Count()));

    //                    block.Edges.Add(newEdge);
    //                    block.Indexes.Add(newEdge);

    //                    freeIndexes.Remove(newEdge);

    //                    dict.Add(newEdge, block);

    //                    var neighborBlocks = GetNeighborBlock(newEdge, dict);
    //                    block.Neighbors.Union(neighborBlocks);
    //                    foreach (var neighborBlock in neighborBlocks)
    //                    {
    //                        neighborBlock.Neighbors.Add(block);
    //                    }

    //                    break;
    //                }
    //            }

    //            if (validEgdes.Count() == 0)
    //            {
    //                finishedBlocks.Add(block);
    //            }
    //        }
    //    }

    //    return dict.Values.Distinct();

    //}

    //private IEnumerable<Block> GetNeighborBlock(Index newEdge, Dictionary<Index, Block> dict)
    //{
    //    var rslt = new HashSet<Block>();
    //    foreach (var index in IndexMethods.GetNeighborCells4(newEdge).Values)
    //    {
    //        if (dict.TryGetValue(index, out var block))
    //        {
    //            rslt.Add(block);
    //        }
    //    }
    //    return rslt;
    //}
}

//public class Block
//{
//    public Index coreIndex { get; set; }
//    public List<Index> Edges { get; set; } = new List<Index>();
//    public List<Index> InvaildEdges { get; set; } = new List<Index>();
//    public List<Index> Indexes { get; set; } = new List<Index>();
//    public HashSet<Block> Neighbors { get; set; } = new HashSet<Block>();
//}
