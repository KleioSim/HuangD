using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Maps;
using System.Collections.Generic;

public partial class PolitcalMap : TileMapLayer
{
    internal void Refresh()
    {
        this.Clear();

        foreach (var province in this.GetSession().Provinces.Values)
        {
            var coreIndex = province.Block.coreIndex;
            this.SetCell(new Vector2I(coreIndex.X, coreIndex.Y), 0, Vector2I.Zero, 0);
        }
    }
}
