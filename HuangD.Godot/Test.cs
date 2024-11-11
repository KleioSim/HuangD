using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Chrona.Engine.Godot.UBBCodes;
using Godot;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Test : Control
{
    public TileMapLayer layer => GetNode<TileMapLayer>("TileMapLayer");

    public override void _Process(double delta)
    {
        var obj = layer.GetCellTileData(new Vector2I(0, 0));
    }
}
