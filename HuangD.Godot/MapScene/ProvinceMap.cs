using Godot;
using System.Collections.Generic;
using System;
using System.Linq;
using HuangD.Sessions.Maps;
using Index = HuangD.Sessions.Maps.Index;

public partial class ProvinceMap : TileMap
{
    private TileMap PoliticalMap => GetNode<TileMap>("PoliticalMap");

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

        var cellsWithDistance = cells.Select(c =>
        {
            var distances = edges.Select(e => (e - c).Length());
            return (cell: c, minDistance: distances.Min(), maxDistance: distances.Max());
        });

        var centerCellWithDistance = cellsWithDistance.GroupBy(x => x.minDistance)
            .MaxBy(x => x.Key)
            .MaxBy(x => x.maxDistance);

        if (centerCellWithDistance.minDistance > 1)
        {
            return this.MapToLocal(centerCellWithDistance.cell);
        }

        var expendCells = cells.SelectMany(c =>
        {
            var baseCells = new Vector2I[]
            {
                new Vector2I(0,0),
                new Vector2I(0,1),
                new Vector2I(1,0),
                new Vector2I(1,1)
            };

            return baseCells.Select(x => c * 2 + x);
        }).ToArray();

        edges = expendCells.Where(c =>
        {
            var neighbors = MapCell.IndexMethods.GetNeighborCells(new Index(c.X, c.Y)).Values
                .Select(n => new Vector2I(n.X, n.Y));
            return neighbors.Any(n => !expendCells.Contains(n));
        });

        cellsWithDistance = expendCells.Select(c =>
        {
            var distances = edges.Select(e => (e - c).Length());
            return (cell: c, minDistance: distances.Min(), maxDistance: distances.Max());
        });

        var centerCell = cellsWithDistance.GroupBy(x => x.minDistance)
            .MaxBy(x => x.Key)
            .MaxBy(x => x.maxDistance)
            .cell;

        return PoliticalMap.MapToLocal(centerCell);

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