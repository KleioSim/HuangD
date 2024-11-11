using Godot;
using System;
using System.Collections.Generic;

public partial class BlockMap : Node2D
{
    private Random random = new System.Random();

    private TileMapLayer TileMapLayer => GetNode<TileMapLayer>("TileMapLayer");

    internal void AddOrUpdate(IEnumerable<HuangD.Sessions.Maps.Index> indexes, string id)
    {
        var newTileMapLayer = TileMapLayer.Duplicate() as TileMapLayer;
        TileMapLayer.AddSibling(newTileMapLayer);

        newTileMapLayer.Name = id;
        newTileMapLayer.Modulate = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);

        foreach (var index in indexes)
        {
            newTileMapLayer.SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }
    }
}