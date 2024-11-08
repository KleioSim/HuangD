using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using Index = HuangD.Sessions.Maps.Index;

public partial class ProvinceMap : Node2D
{
    public Vector2I TileSize => TileMapLayer.TileSet.TileSize;

    private TileMapLayer TileMapLayer => GetNode<TileMapLayer>("TileMapLayer");

    private Random random = new Random();
    private List<Color> colors = new List<Color>();
    private Dictionary<string, TileMapLayer> tileMapLayers = new Dictionary<string, TileMapLayer>();

    internal void AddOrUpdate(IEnumerable<Index> indexes, string provinceId)
    {
        while (true)
        {
            var color = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);
            if (!colors.Contains(color))
            {
                colors.Add(color);
                break;
            }
        }

        var newTileMapLayer = TileMapLayer.Duplicate() as TileMapLayer;
        TileMapLayer.AddSibling(newTileMapLayer);
        tileMapLayers.Add(provinceId, newTileMapLayer);

        newTileMapLayer.Name = provinceId;
        newTileMapLayer.Modulate = colors.Last();

        foreach (var index in indexes)
        {
            newTileMapLayer.SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }
    }

    internal Vector2 MapToLocal(Vector2I vector2I)
    {
        return TileMapLayer.MapToLocal(vector2I);
    }

    internal Vector2I LocalToMap(Vector2 vector)
    {
        return TileMapLayer.LocalToMap(vector);
    }

    internal string GetCellProvinceId(Vector2I vector)
    {
        var layer = tileMapLayers.Values.FirstOrDefault(x => x.GetCellSourceId(vector) != -1);
        return layer != null ? layer.Name : null;
    }

    internal void Clear()
    {
        foreach (var oldLayer in tileMapLayers.Values)
        {
            oldLayer.QueueFree();
        }

        tileMapLayers.Clear();
    }
}