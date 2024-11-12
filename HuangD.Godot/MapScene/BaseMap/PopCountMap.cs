using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PopCountMap : Node2D
{
    private TileMapLayer TileMapLayer => GetNode<TileMapLayer>("TileMapLayer");


    public override void _Ready()
    {
        TileMapLayer.Modulate = new Color(1f, 1f, 1f);

        for (int i = 0; i < 10; i++)
        {
            var newTileMapLayer = TileMapLayer.Duplicate() as TileMapLayer;
            TileMapLayer.AddSibling(newTileMapLayer);

            newTileMapLayer.Modulate = new Color(1f, (10 - i) * 0.1f, (10 - i) * 0.1f);
        }
    }

    internal void Refresh()
    {
        Clear();

        var layers = TileMapLayer.GetParent().GetChildren().OfType<TileMapLayer>().ToArray();

        var session = this.GetSession();
        foreach (var province in session.Provinces.Values)
        {
            var layerId = province.PopCount * 10 / session.Provinces.Values.Max(p => p.PopCount);
            foreach (var index in province.Block.Indexes)
            {
                layers[layerId].SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
            }
        }
    }


    private void Clear()
    {
        TileMapLayer.Clear();

        var layers = TileMapLayer.GetParent().GetChildren().OfType<TileMapLayer>().ToArray();
        foreach (var layer in layers)
        {
            layer.Clear();
        }
    }
}
