using Godot;
using Godot.Collections;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Maps;
using System;
using System.Drawing;
using System.Linq;

public partial class BoundaryMap : TileMapLayer
{
    internal void Refresh(Vector2I scale)
    {
        this.Clear();

        var mapSize = new Vector2I(this.GetSession().MapSize.x, this.GetSession().MapSize.y) * scale;

        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                var index = new Vector2I(x, y);
                this.SetCell(index, 0, Vector2I.Zero, 0);
            }
        }

    }

    internal void Update(Vector2I size, ProvinceMap provinceMap)
    {
        this.Clear();

        var tileSize = TileSet.TileSize;
        var provTileSize = provinceMap.TileSize;

        for (int i = -1; i <= size.X; i++)
        {
            this.SetCell(new Vector2I(i, -1), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(i, size.Y), 0, Vector2I.Zero, 0);
        }

        for (int i = -1; i <= size.Y; i++)
        {
            this.SetCell(new Vector2I(-1, i), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(size.Y, i), 0, Vector2I.Zero, 0);
        }


        var dist = tileSize / 2;
        var array = new[] { dist, dist * -1, new Vector2I(dist.X, dist.Y * -1), new Vector2I(dist.X * -1, dist.Y) };

        for (int x = 0; x < size.X; x++)
        {
            for (int y = 0; y < size.Y; y++)
            {
                var index = new Vector2I(x, y);
                var centerPos = ToGlobal(this.MapToLocal(index));
                var pos2 = array.Select(x => (x + centerPos)).ToArray();
                var pos3 = pos2.Select(x => provinceMap.ToLocal(x)).ToArray();
                var pos4 = pos3.Select(x => provinceMap.LocalToMap(x)).ToArray();
                var pos5 = pos4.Select(x => provinceMap.GetCellProvinceId(x)).Where(x => x != null);
                if (pos5.Distinct().Count() > 1)
                {
                    this.SetCell(index, 0, Vector2I.Zero, 0);
                }
            }
        }
    }
}
