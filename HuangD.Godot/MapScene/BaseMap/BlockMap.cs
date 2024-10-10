using Godot;
using System;
using System.Collections.Generic;

public partial class BlockMap : TileMap
{
    private Random random = new System.Random();

    private int layerId;

    internal void AddOrUpdate(List<HuangD.Sessions.Maps.Index> indexes)
    {
        var color = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);
        AddLayer(layerId);
        SetLayerModulate(layerId, color);

        foreach (var index in indexes)
        {
            SetCell(layerId, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }

        layerId++;
    }
}