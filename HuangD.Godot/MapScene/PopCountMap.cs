using Godot;
using HuangD.Sessions.Maps;
using System;

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

    public void AddOrUpdate(HuangD.Sessions.Maps.Index index, int Count)
    {
        this.SetCell(Math.Min(Count / 1000, 10), new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
    }
}
