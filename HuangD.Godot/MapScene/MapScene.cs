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
    }

    private void OnTerrainAdd(TerrainItem item)
    {
        var terrainMap = GetNode<TerrainMap>("TerrainMap");
        terrainMap.AddOrUpdate(item.Index, item.Type);
    }

    public override void _Process(double delta)
    {

    }
}
