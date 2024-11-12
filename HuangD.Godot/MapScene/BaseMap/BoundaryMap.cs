using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Linq;
using Index = HuangD.Sessions.Maps.Index;

public partial class BoundaryMap : TileMapLayer
{
    internal void Refresh(Vector2I scale)
    {
        this.Clear();

        var mapSize = new Vector2I(this.GetSession().MapSize.x, this.GetSession().MapSize.y) * scale;

        var offsets = new Vector2[] {
            new Vector2(1, 1),
            new Vector2(1, -1),
            new Vector2(-1, 1),
            new Vector2(-1, -1),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, -1) };

        for (int x = 0; x < mapSize.X; x++)
        {
            for (int y = 0; y < mapSize.Y; y++)
            {
                var vector = new Vector2I(x, y);

                if (x % scale.X != 0 && y % scale.Y != 0)
                {
                    continue;
                }

                var provinces = offsets.Select(o => o + vector - Vector2I.One)
                    .Select(o => new Vector2(o.X / scale.X, o.Y / scale.Y))
                    .Where(v => v.X == Math.Floor(v.X) && v.Y == Math.Floor(v.Y))
                    .Select(o => this.GetSession().Provinces.GetByIndex(new Index((int)o.X, (int)o.Y)))
                    .Where(p => p != null)
                    .ToArray();

                if (provinces.Distinct().Count() > 1)
                {
                    this.SetCell(vector, 0, Vector2I.Zero, 0);
                }
            }
        }

    }
}
