using Godot;
using System;

public partial class EdgeMap : TileMapLayer
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
    }
}
