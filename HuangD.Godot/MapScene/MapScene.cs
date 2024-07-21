using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Maps;
using System;
using System.Reactive.Linq;

public partial class MapScene : Node2D
{
    public override void _Ready()
    {
        var session = this.GetSession();
        session.Map.Terrains.Connect().Subscribe(OnTerrainAdd, null, null).EndWith(this, SignalName.TreeExiting);
        session.Map.Pops.Connect().Subscribe(OnPopCountAdd, null, null).EndWith(this, SignalName.TreeExiting);

        var camera = GetNode<Camera2D>("CanvasLayer/Camera2D");
        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        camera.Position = terrainMap.MapToLocal(terrainMap.GetUsedRect().GetCenter());
    }

    private void OnPopCountAdd(PopItem item)
    {
        var popCountMap = GetNode<PopCountMap>("CanvasLayer/PopCountMap");
        popCountMap.AddOrUpdate(item.Index, item.Count);
    }

    private void OnTerrainAdd(TerrainItem item)
    {
        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        terrainMap.AddOrUpdate(item.Index, item.Type);
    }
}
