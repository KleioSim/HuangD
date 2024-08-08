using Godot;
using System.Collections.Generic;
using System;
using System.Linq;
using HuangD.Sessions.Maps;
using Index = HuangD.Sessions.Maps.Index;

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


        var edges = cells.Where(c =>
        {
            var neighbors = MapCell.IndexMethods.GetNeighborCells(new Index(c.X, c.Y)).Values
                .Select(n => new Vector2I(n.X, n.Y));
            return neighbors.Any(n => GetCellSourceId(dict[key], n) == -1);
        });

        var centerCell = cells.First();
        var distance = int.MinValue;
        foreach (var cell in cells)
        {
            var minDist = (int)edges.Select(e => (e - cell).Length()).Min();
            if (minDist > distance)
            {
                distance = minDist;
                centerCell = cell;
            }
        }

        return MapToLocal(centerCell);
    }

    internal string LocalToProvince(Vector2 vector2)
    {
        var cellVector = LocalToMap(vector2);

        foreach(var pair in dict)
        {
            if(GetCellSourceId(pair.Value, cellVector) != -1)
            {
                return pair.Key;
            }
        }

        return null;
    }
}