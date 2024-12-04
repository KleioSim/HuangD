using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using HuangD.Sessions.Maps;
using Index = HuangD.Sessions.Maps.Index;

public partial class MapManager : Node2D
{
    TileMapLayer TileMap128 => GetNode<TileMapLayer>("TileMapLayer128");
    TileMapLayer TileMapLayerBlueprint => GetNode<TileMapLayer>("TileMapLayerBlueprint");

    public void BuildBlueprint()
    {
        TileMapLayerBlueprint.Clear();
        for(int i=0; i<40; i++)
        {
            for(int j=0; j<40; j++)
            {
                TileMapLayerBlueprint.SetCell(new Vector2I(i, j), 0, Vector2I.Zero, 0);
            }
        }
    }

    public void BuildBlockCore()
    {
        TileMap128.Clear();
        var coreCells = GenerateCoreIndex(40, 40, new Random(), 5);
        foreach (var coreCell in coreCells)
        {
            TileMap128.SetCell(new Vector2I(coreCell.X, coreCell.Y), 0, Vector2I.Zero, 0);
        }
    }

    private static List<Index> GenerateCoreIndex(int high, int width, System.Random random, int cellRadius)
    {
        var coreIndexs = new List<Index>();

        var offset = cellRadius / 2;

        for (int i=0; i< high / cellRadius; i++)
        {
            for(int j=0; j< width / cellRadius; j++)
            {
                coreIndexs.Add(new Index(i* cellRadius + random.Next(offset * -1, offset), j* cellRadius + random.Next(offset * -1, offset)));
            }
        }

        //var fullIndexes = new Queue<Index>(Enumerable.Range(cellRadius, width - cellRadius)
        //    .SelectMany(x => Enumerable.Range(cellRadius, high - cellRadius).Select(y => new Index(x, y)))
        //    .OrderBy(_ => random.Next()));

        //while (fullIndexes.Count != 0)
        //{
        //    var curr = fullIndexes.Dequeue();
        //    if (coreIndexs.Any(index => System.Math.Abs(index.X - curr.X) < cellRadius * 2 && System.Math.Abs(index.Y - curr.Y) < cellRadius * 2))
        //    {
        //        continue;
        //    }

        //    coreIndexs.Add(curr);
        //}

        return coreIndexs.ToList();
    }
}
