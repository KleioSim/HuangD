using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

public partial class ProvinceMap : TileMap
{
    private Random random = new Random();
    private Dictionary<string, int> dict = new Dictionary<string, int>();
    private List<Color> colors = new List<Color>();

    public override void _Ready()
    {

    }

    public void AddOrUpdate(HuangD.Sessions.Maps.Index index, string provinceId)
    {

        if (!dict.TryGetValue(provinceId, out int layerId))
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

            layerId = colors.Count() - 1;
            this.AddLayer(layerId);
            this.SetLayerModulate(layerId, colors.ElementAt(layerId));

            dict.Add(provinceId, layerId);
        }


        this.SetCell(layerId, new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
    }

    internal Vector2 GetPawnLocation(string key)
    {
        var cells = this.GetUsedCells(dict[key]);
        return MapToLocal(cells.First());
    }
}