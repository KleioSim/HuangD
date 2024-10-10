using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using Index = HuangD.Sessions.Maps.Index;

public partial class ProvinceMap : TileMap
{
    private Random random = new Random();
    private List<Color> colors = new List<Color>();

    public override void _Ready()
    {

    }

    internal void AddOrUpdate(List<Index> indexes, string provinceId)
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

        this.AddLayer(-1);

        var layerId = this.GetLayersCount() - 1;
        this.SetLayerName(layerId, provinceId);
        this.SetLayerModulate(layerId, colors.Last());

        foreach (var index in indexes)
        {
            this.SetCell(layerId, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }
    }
}