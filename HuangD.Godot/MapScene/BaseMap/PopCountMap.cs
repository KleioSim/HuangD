using Godot;
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
        for (int i = 0; i < 10; i++)
        {
            var newTileMapLayer = TileMapLayer.Duplicate() as TileMapLayer;
            TileMapLayer.AddSibling(newTileMapLayer);
        }
    }

    internal void AddOrUpdate(IEnumerable<HuangD.Sessions.Maps.Index> indexes, int layerId)
    {
        var layers = TileMapLayer.GetParent().GetChildren().OfType<TileMapLayer>().ToArray();

        layers[layerId].Modulate = new Color(1f, (10 - layerId) * 0.1f, (10 - layerId) * 0.1f);

        foreach (var index in indexes)
        {
            layers[layerId].SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }

    }
}
