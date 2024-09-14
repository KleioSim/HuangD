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
        var blocks = BuildBlocks(100, 500);

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
        //var list = new List<Block>()
        //{
        //    new Block()
        //    {
        //        Indexes = Enumerable.Range(0, 3).SelectMany(x => Enumerable.Range(0, 3).Select(y => new Index(x, y))).ToArray()
        //    },
        //    new Block()
        //    {
        //        Indexes = Enumerable.Range(3, 3).SelectMany(x => Enumerable.Range(3, 3).Select(y => new Index(x, y))).ToArray()
        //    }
        //};

        var a = new HashSet<Index>() { new Index(0, 0) };
        var b = new HashSet<Index>() { new Index(0, 0), new Index(0, 1) };

        if (a.IsSubsetOf(b))
        {
            GD.Print();
        }

        if (b.IsSubsetOf(a))
        {
            GD.Print();
        }

        var list = new List<Block>();

        var random = new System.Random();
        var freeIndexes = Enumerable.Range(0, width).SelectMany(x => Enumerable.Range(0, high).Select(y => new Index(x, y))).ToHashSet();
        var usedIndexes = new HashSet<Index>();
        var deadStartIndexes = new HashSet<Index>();

        while (true)
        {

            Index startIndex = null;
            foreach (var index in freeIndexes)
            {
                if (deadStartIndexes.Contains(index))
                {
                    continue;
                }

                if (IndexMethods.GetNeighborCells4(index).Values.ToHashSet().IsSubsetOf(freeIndexes))
                {
                    startIndex = index;
                    break;
                }

                deadStartIndexes.Add(index);
            }

            if (startIndex == null)
            {
                break;
            }


            var edgeIndexes = IndexMethods.GetNeighborCells4(startIndex).Values.ToHashSet();
            var blockIndexes = edgeIndexes.Prepend(startIndex).ToHashSet();

            usedIndexes.UnionWith(blockIndexes);
            freeIndexes.ExceptWith(blockIndexes);

            while (blockIndexes.Count() < 32)
            {
                IEnumerable<Index> newEdgeIndexs = null;
                foreach (var index in edgeIndexes.OrderBy(_ => random.Next()))
                {
                    var neighbors = IndexMethods.GetNeighborCells4(index).Values
                        .Intersect(freeIndexes).ToArray();

                    newEdgeIndexs = neighbors.OrderBy(_ => random.Next()).Take(System.Math.Min(3, neighbors.Length));
                    if (!newEdgeIndexs.Any())
                    {
                        continue;
                    }

                    edgeIndexes.UnionWith(newEdgeIndexs);
                    blockIndexes.UnionWith(newEdgeIndexs);

                    usedIndexes.UnionWith(newEdgeIndexs);
                    freeIndexes.ExceptWith(newEdgeIndexs);

                    if (neighbors.Length == 1)
                    {
                        edgeIndexes.Remove(index);
                    }

                    break;
                }

                if (newEdgeIndexs == null || !newEdgeIndexs.Any())
                {
                    break;
                }
            }

            var block = new Block() { Indexes = blockIndexes };
            list.Add(block);
        }

        return list;

    }
}

public class Block
{
    public IEnumerable<Index> Indexes { get; set; }
}
