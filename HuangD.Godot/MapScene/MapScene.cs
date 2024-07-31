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
    MapCamera2D Camera => GetNode<MapCamera2D>("CanvasLayer/Camera2D");
    InstancePlaceholder PoliticalInfo => GetNode<InstancePlaceholder>("CanvasLayer/PoliticalInfo");

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

        foreach(var province in session.Provinces.Values)
        {
            var politicalInfo = PoliticalInfo.CreateInstance() as PoliticalInfo;
            politicalInfo.Position = ProvinceMap.GetPawnLocation(province.Key) ;
            politicalInfo.OnZoomed(Camera.Zoom);
        }

        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        Camera.Position = terrainMap.MapToLocal(terrainMap.GetUsedRect().GetCenter());
    }
}
