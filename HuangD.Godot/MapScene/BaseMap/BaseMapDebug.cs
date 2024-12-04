using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Index = HuangD.Sessions.Maps.Index;
public partial class BaseMapDebug : Control
{
    public GraphElement GraphElement => GetNode<GraphElement>("MarginContainer/GraphEdit/GraphElement");
    public TileMapLayer Blueprint => GetNode<TileMapLayer>("MarginContainer/GraphEdit/GraphElement/BaseMap/Blueprint");
    public TileMapLayer BlockMap128 => GetNode<TileMapLayer>("MarginContainer/GraphEdit/GraphElement/BaseMap/BlockMap/128/TileMapLayer");
    public TileMapLayer BlockMap512 => GetNode<TileMapLayer>("MarginContainer/GraphEdit/GraphElement/BaseMap/BlockMap/512/TileMapLayer");
    public TileMapLayer BlockMap256 => GetNode<TileMapLayer>("MarginContainer/GraphEdit/GraphElement/BaseMap/BlockMap/256/TileMapLayer");

    public void BuildBlueprint()
    {
        var vectors = Enumerable.Range(0, 40)
            .SelectMany(x => Enumerable.Range(0, 40).Select(y => new Vector2I(x, y)))
            .ToArray();
        Blueprint.Clear();
        foreach (var vector in vectors)
        {
            Blueprint.SetCell(vector, 0, Vector2I.Zero, 0);
        }

        GraphElement.Size = Blueprint.MapToLocal(Blueprint.GetUsedRect().Size);
    }

    public void BuildBlockCore()
    {
        var vector512s = Enumerable.Range(0, 10)
            .SelectMany(x => Enumerable.Range(0, 10).Select(y => new Vector2I(x, y)))
            .ToArray();

        var random = new Random();
        var colors = vector512s.ToDictionary(x => x, _ => new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f));

        var tilemapLayers = new Queue<TileMapLayer>(BlockMap512.GetParent().GetChildren().OfType<TileMapLayer>());
        foreach (var vector in vector512s)
        {
            if (!tilemapLayers.TryDequeue(out var layer))
            {
                layer = BlockMap512.Duplicate() as TileMapLayer;
                BlockMap512.AddSibling(layer);
            }

            layer.Modulate = colors[vector];

            layer.Clear();
            layer.SetCell(vector, 0, Vector2I.Zero, 0);
        }

        var vector256s = Enumerable.Range(0, 20)
            .SelectMany(x => Enumerable.Range(0, 20).Select(y => new Vector2I(x, y)))
            .ToArray();
        tilemapLayers = new Queue<TileMapLayer>(BlockMap256.GetParent().GetChildren().OfType<TileMapLayer>());
        foreach (var vector in vector256s)
        {
            var fractalUpVectors = FractalUp(vector);

            if (!tilemapLayers.TryDequeue(out var layer))
            {
                layer = BlockMap256.Duplicate() as TileMapLayer;
                BlockMap256.AddSibling(layer);
            }


            if (fractalUpVectors.Count() < 1)
            {
                continue;
            }

            if (fractalUpVectors.Count() > 2)
            {
                throw new Exception();
            }

            var prefColors = new List<Color>();
            if (colors.ContainsKey(fractalUpVectors.First()))
            {
                prefColors.Add(colors[fractalUpVectors.First()]);
            }
            if (colors.ContainsKey(fractalUpVectors.Last()))
            {
                prefColors.Add(colors[fractalUpVectors.Last()]);
            }

            layer.Modulate = prefColors.Count == 0 ? new Color(1, 1, 1) : prefColors[random.Next(0, prefColors.Count)];

            layer.Clear();
            layer.SetCell(vector, 0, Vector2I.Zero, 0);
        }
    }

    private IEnumerable<Vector2I> FractalUp(Vector2I vector)
    {
        var rslt = new HashSet<Vector2I>();
        if (vector.X % 4 == 0)
        {
            if(vector.Y % 2 == 0)
            {
                rslt.Add(vector / 2);
                return rslt;
            }
            else
            {
                rslt.Add(new Vector2I(vector.X / 2, (vector.Y + 1) / 2));
                rslt.Add(new Vector2I(vector.X / 2, (vector.Y - 1) / 2));
            }

            return rslt;
        }

        if(vector.X % 2 == 0)
        {
            var Y = vector.Y - 1;
            if (Y % 2 == 0)
            {
                rslt.Add(new Vector2I(vector.X / 2, Y / 2));
                return rslt;
            }
            else
            {
                rslt.Add(new Vector2I(vector.X / 2, (vector.Y + 1) / 2));
                rslt.Add(new Vector2I(vector.X / 2, (vector.Y - 1) / 2));
            }

            return rslt;
        }

        var xArray = new int[] { (vector.X - 1) / 2, (vector.X + 1) / 2 };
        var yArray = new int[] { };
        if(vector.Y % 2 == 0)
        {
            yArray = new int[] { vector.Y / 2, vector.Y / 2 };
        }
        else  if(vector.X % 3 == 0)
        {
            yArray = new int[] { (vector.Y - 1) / 2, (vector.Y + 1) / 2 };
        }
        else
        {
            yArray = new int[] { (vector.Y + 1) / 2, (vector.Y - 1) / 2 };
        }

        for(int i=0; i<xArray.Length; i++)
        {
            rslt.Add(new Vector2I(xArray[i], yArray[i]));
        }
        return rslt;
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
}
