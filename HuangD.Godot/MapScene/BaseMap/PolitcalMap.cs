using Godot;
using HuangD.Sessions.Maps;
using System.Collections.Generic;

public partial class PolitcalMap : TileMapLayer
{
    internal void AddOrUpdate(Index coreIndex, string provinceId)
    {
        this.SetCell(new Vector2I(coreIndex.X, coreIndex.Y), 0, Vector2I.Zero, 0);

        GD.Print($"PolitcalMap {coreIndex}");
    }
}
