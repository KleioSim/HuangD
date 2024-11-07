using Godot;
using HuangD.Sessions.Maps;
using System;
using System.Linq;

public partial class BoundaryMap : TileMapLayer
{
    internal void Update(ProvinceMap provinceMap)
    {
        this.Clear();

        var tileSize = TileSet.TileSize;
        var provTileSize = provinceMap.TileSet.TileSize;

        var MapSize = provinceMap.GetUsedRect().Size * provTileSize / tileSize;

        for (int i = -1; i <= MapSize.X; i++)
        {
            this.SetCell(new Vector2I(i, -1), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(i, MapSize.Y), 0, Vector2I.Zero, 0);
        }

        for (int i = -1; i <= MapSize.Y; i++)
        {
            this.SetCell(new Vector2I(-1, i), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(MapSize.Y, i), 0, Vector2I.Zero, 0);
        }


        var dist = tileSize / 2;
        var array = new[] { dist, dist * -1, new Vector2I(dist.X, dist.Y * -1), new Vector2I(dist.X * -1, dist.Y) };

        for (int x = 0; x < MapSize.X; x++)
        {
            for (int y = 0; y < MapSize.Y; y++)
            {
                var index = new Vector2I(x, y);
                var centerPos = ToGlobal(this.MapToLocal(index));
                var pos2 = array.Select(x => (x + centerPos)).ToArray();
                var pos3 = pos2.Select(x => provinceMap.ToLocal(x)).ToArray();
                var pos4 = pos3.Select(x => provinceMap.LocalToMap(x)).ToArray();
                var pos5 = pos4.Select(x =>
                {
                    var layerId = -1;
                    for (int i = 0; i < provinceMap.GetLayersCount(); i++)
                    {
                        var sourceId = provinceMap.GetCellSourceId(i, x);
                        if (sourceId != -1)
                        {
                            layerId = i;
                            break;
                        }
                    }
                    return layerId;
                }).ToArray();

                if (pos5.Distinct().Count() > 1)
                {
                    this.SetCell(index, 0, Vector2I.Zero, 0);
                }
            }
        }
    }
}
