using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Maps;
using System;
using System.Reactive.Linq;

public partial class MapScene : Node2D
{
    ProvinceMap ProvinceMap => GetNode<ProvinceMap>("CanvasLayer/ProvinceMap");
    PopCountMap PopCountMap => GetNode<PopCountMap>("CanvasLayer/PopCountMap");
    TerrainMap TerrainMap => GetNode<TerrainMap>("CanvasLayer/TerrainMap");
    public override void _Ready()
    {
        var session = this.GetSession();

        foreach(var cell in session.MapCells.Values)
        {
            TerrainMap.AddOrUpdate(cell.Index, cell.TerrainType);
            if(cell.TerrainType != TerrainType.Water) 
            {
                PopCountMap.AddOrUpdate(cell.Index, cell.PopCount);
                ProvinceMap.AddOrUpdate(cell.Index, cell.ProvinceId);
            }
        }

        var camera = GetNode<Camera2D>("CanvasLayer/Camera2D");
        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        camera.Position = terrainMap.MapToLocal(terrainMap.GetUsedRect().GetCenter());
    }
}
