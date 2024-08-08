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
    InstancePlaceholder PoliticalInfoPlaceHolder => GetNode<InstancePlaceholder>("CanvasLayer/PoliticalInfo");

    [Signal]
    public delegate void ClickProvinceEventHandler(string id);

    public override void _Ready()
    {
        var session = this.GetSession();

        foreach (var cell in session.MapCells.Values)
        {
            TerrainMap.AddOrUpdate(cell.Index, cell.TerrainType);
            if (cell.TerrainType != TerrainType.Water)
            {
                PopCountMap.AddOrUpdate(cell.Index, cell.PopCount);
                ProvinceMap.AddOrUpdate(cell.Index, cell.ProvinceId);
            }
        }

        foreach (var province in session.Provinces.Values)
        {
            var politicalInfo = PoliticalInfoPlaceHolder.CreateInstance() as PoliticalInfo;
            politicalInfo.Position = ProvinceMap.GetPawnLocation(province.Key);
            politicalInfo.province = province;

            politicalInfo.OnZoomed(Camera.Zoom);

            Camera.Connect(MapCamera2D.SignalName.OnZoomed, new Callable(politicalInfo, PoliticalInfo.MethodName.OnZoomed));
        }

        var terrainMap = GetNode<TerrainMap>("CanvasLayer/TerrainMap");
        Camera.Position = terrainMap.MapToLocal(terrainMap.GetUsedRect().GetCenter());
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventKey)
        {
            if (eventKey.Pressed)
            {
                if (eventKey.ButtonIndex == MouseButton.Left)
                {
                    var provinceId = ProvinceMap.LocalToProvince(GetGlobalMousePosition());

                    if (provinceId != null)
                    {
                        EmitSignal(SignalName.ClickProvince, provinceId);
                        GD.Print(provinceId);
                    }
                }
            }
            return;
        }
    }
}
