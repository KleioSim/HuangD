using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Maps;
using System;
using System.Reactive.Linq;

public partial class MapScene : Control
{
    public override void _Ready()
    {
        var session = this.GetSession();
        session.Map.Terrains.Connect().Subscribe(OnTerrainAdd, null, null).EndWith(this, SignalName.TreeExiting);
        session.Map.Pops.Connect().Subscribe(OnPopCountAdd, null, null).EndWith(this, SignalName.TreeExiting);
    }

    private void OnPopCountAdd(PopItem item)
    {
        var popCountMap = GetNode<PopCountMap>("CanvasLayer/TerrainMap");
        popCountMap.AddOrUpdate(item.Index, item.Count);
    }

    private void OnTerrainAdd(TerrainItem item)
    {
        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        terrainMap.AddOrUpdate(item.Index, item.Type);
    }
}
