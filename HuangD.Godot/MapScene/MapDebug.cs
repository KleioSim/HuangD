using Godot;
using HuangD.Sessions.Maps;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public partial class MapDebug : Node2D
{
    TileMap TileMap => GetNode<TileMap>("TileMap");

    IIndexMethods IndexMethods = new SquareIndexMethods();

    public override void _Ready()
    {
        var blocks = BuildBlocks(160, 90);

        var random = new System.Random();

        for (int i = 0; i < blocks.Count(); i++)
        {
            var color = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);
            TileMap.AddLayer(i);
            TileMap.SetLayerModulate(i, color);

            foreach (var index in blocks.ElementAt(i).Indexes)
            {
                TileMap.SetCell(i, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
            }
        }

    }

    private IEnumerable<Block> BuildBlocks(int width, int high)
    {
        var random = new System.Random();

        var list = new List<Block>();

        var cellRadius = 4;

        for (int i = cellRadius; i < width; i += cellRadius * 2 + 1)
        {
            for (int j = cellRadius; j < high; j += cellRadius * 2 + 1)
            {
                var ceneter = new Index(random.Next(i - cellRadius / 2, i + cellRadius / 2 + 1), random.Next(j - cellRadius / 2, j + cellRadius / 2 + 1));

                var block = new Block();
                block.Edges = IndexMethods.GetNeighborCells(ceneter).Values.ToList();
                block.Indexes = block.Edges.Append(ceneter).ToList();
                list.Add(block);
            }
        }

        var freeIndexes = Enumerable.Range(0, width).SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y))).ToHashSet();
        freeIndexes.ExceptWith(list.SelectMany(x => x.Indexes));


        var finishedBlocks = new HashSet<Block>();

        while (freeIndexes.Count != 0 && finishedBlocks.Count < list.Count)
        {
            foreach (var block in list.Except(finishedBlocks))
            {

                while (block.Edges.Count != 0)
                {
                    var edge = block.Edges.ElementAt(random.Next(0, block.Edges.Count()));

                    var newEdges = IndexMethods.GetNeighborCells4(edge).Values
                        .Where(x => freeIndexes.Contains(x))
                        .ToArray();
                    if (newEdges.Length == 0)
                    {
                        block.Edges.Remove(edge);
                    }
                    else
                    {
                        var newEdge = newEdges.ElementAt(random.Next(0, newEdges.Count()));

                        block.Edges.Add(newEdge);
                        block.Indexes.Add(newEdge);

                        freeIndexes.Remove(newEdge);

                        break;
                    }
                }

                if (block.Edges.Count == 0 || block.Indexes.Count > (cellRadius * 3) * (cellRadius * 2))
                {
                    finishedBlocks.Add(block);
                }
            }
        }

        return list;


        //var random = new System.Random();
        //var range = (X: random.Next(7, width - 7), Y: random.Next(7, high - 7));

        //var list = new List<Block>() { new Block(), new Block(), new Block(), new Block() };
        //for(int i=0; i<width; i++)
        //{
        //    for(int j=0; j<high; j++)
        //    {
        //        if(i<=range.X && j <= range.Y)
        //        {
        //            list[0].Indexes.Add(new Index(i, j));
        //        }
        //        else if (i > range.X && j <= range.Y)
        //        {
        //            list[1].Indexes.Add(new Index(i, j));
        //        }
        //        else if (i <= range.X && j > range.Y)
        //        {
        //            list[2].Indexes.Add(new Index(i, j));
        //        }
        //        if (i > range.X && j > range.Y)
        //        {
        //            list[3].Indexes.Add(new Index(i, j));
        //        }
        //    }
        //}

        //return list;

        //var a = new HashSet<Index>() { new Index(0, 0) };
        //var b = new HashSet<Index>() { new Index(0, 0), new Index(0, 1) };

        //if (a.IsSubsetOf(b))
        //{
        //    GD.Print();
        //}

        //if (b.IsSubsetOf(a))
        //{
        //    GD.Print();
        //}

        //var list = new List<Block>();

        //var random = new System.Random();
        //var freeIndexes = Enumerable.Range(0, width).SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y))).ToHashSet();
        //var usedIndexes = new HashSet<Index>();
        //var deadStartIndexes = new HashSet<Index>();

        //while (true)
        //{

        //    Index startIndex = null;
        //    foreach (var index in freeIndexes)
        //    {
        //        if (deadStartIndexes.Contains(index))
        //        {
        //            continue;
        //        }

        //        if (IndexMethods.GetNeighborCells4(index).Values.ToHashSet().All(x => freeIndexes.Contains(x)))
        //        {
        //            startIndex = index;
        //            break;
        //        }

        //        deadStartIndexes.Add(index);
        //        continue;
        //    }

        //    if (startIndex == null)
        //    {
        //        break;
        //    }


        //    var edgeIndexes = IndexMethods.GetNeighborCells4(startIndex).Values.ToHashSet();
        //    var blockIndexes = edgeIndexes.Prepend(startIndex).ToHashSet();

        //    usedIndexes.UnionWith(blockIndexes);
        //    freeIndexes.ExceptWith(blockIndexes);

        //    while (blockIndexes.Count() < 32)
        //    {
        //        IEnumerable<Index> newEdgeIndexs = null;
        //        foreach (var index in edgeIndexes.OrderBy(_ => random.Next()))
        //        {
        //            var neighbors = IndexMethods.GetNeighborCells4(index).Values
        //                .Intersect(freeIndexes).ToArray();

        //            newEdgeIndexs = neighbors.OrderBy(_ => random.Next()).Take(System.Math.Min(3, neighbors.Length));
        //            if (!newEdgeIndexs.Any())
        //            {
        //                continue;
        //            }

        //            edgeIndexes.UnionWith(newEdgeIndexs);
        //            blockIndexes.UnionWith(newEdgeIndexs);

        //            usedIndexes.UnionWith(newEdgeIndexs);
        //            freeIndexes.ExceptWith(newEdgeIndexs);

        //            if (neighbors.Length == 1)
        //            {
        //                edgeIndexes.Remove(index);
        //            }

        //            break;
        //        }

        //        if (newEdgeIndexs == null || !newEdgeIndexs.Any())
        //        {
        //            break;
        //        }
        //    }

        //    var block = new Block() { Indexes = blockIndexes };
        //    list.Add(block);
        //}

        //return list;

    }

    private void BuildBlocks(List<Block> list, int startX, int startY, int endX, int endY)
    {
        //list.Add(new Block() { Indexes = Enumerable.Range(startX, endX).SelectMany(x => Enumerable.Range(startY, endY).Select(y => new Index(x, y))).ToList() });

        if (endX - startX < 6 || endY - startY < 6)
        {
            return;
        }

        var random = new System.Random();
        var range = (X: random.Next(startX + 3, endX - 3), Y: random.Next(startY + 3, endY - 3));

        //var range = (X:startX + (endX - startX)/2, Y:startY + (endY - startY)/2);

        var block = new Block() { Indexes = new List<Index>() { new Index(range.X, range.Y) } };

        list.Add(block);

        BuildBlocks(list, startX, startY, range.X, range.Y);
        BuildBlocks(list, startX, range.Y, range.X, endY);
        BuildBlocks(list, range.X, startY, endX, range.Y);
        BuildBlocks(list, range.X, range.Y, endX, endY);
    }
}

public class Block
{
    public List<Index> Edges { get; set; } = new List<Index>();
    public List<Index> Indexes { get; set; } = new List<Index>();
}
