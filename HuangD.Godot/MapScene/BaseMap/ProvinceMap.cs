using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ProvinceMap : Node2D
{
    public Vector2I TileSize => TileMapLayer.TileSet.TileSize;

    private TileMapLayer TileMapLayer => GetNode<TileMapLayer>("TileMapLayer");

    private Random random = new Random();
    private List<Color> colors = new List<Color>();
    private Dictionary<string, TileMapLayer> tileMapLayers = new Dictionary<string, TileMapLayer>();


    internal void Clear()
    {
        TileMapLayer.Clear();

        foreach (var oldLayer in tileMapLayers.Values)
        {
            oldLayer.QueueFree();
        }

        tileMapLayers.Clear();
    }

    internal void Refresh()
    {
        Clear();

        foreach (var province in this.GetSession().Provinces.Values)
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
            tileMapLayers.Add(province.Id, newTileMapLayer);

            newTileMapLayer.Name = province.Id;
            newTileMapLayer.Modulate = colors.Last();

            foreach (var index in province.Block.Indexes)
            {
                newTileMapLayer.SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
            }
        }
    }
}