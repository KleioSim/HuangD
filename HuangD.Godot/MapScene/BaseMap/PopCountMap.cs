using Godot;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;

public partial class PopCountMap : TileMap
{
    public override void _Ready()
    {
        for (int i = 0; i <= 10; i++)
        {
            this.AddLayer(i);
            this.SetLayerModulate(i, new Color(1f, (10 - i) * 0.1f, (10 - i) * 0.1f));
        }
    }
    internal void AddOrUpdate(IEnumerable<HuangD.Sessions.Maps.Index> indexes, int layerId)
    {
        foreach (var index in indexes)
        {
            SetCell(layerId, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }
    }
}
