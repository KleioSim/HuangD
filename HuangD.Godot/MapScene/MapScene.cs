﻿using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Signal]
    public delegate void ClickArmyEventHandler(string id);

    [Signal]
    public delegate void ClickArmyMoveTargetEventHandler(string id);

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
            politicalInfo.Position = ProvinceMap.GetPawnLocation(province.Id);
            politicalInfo.province = province;
            politicalInfo.ArmyInfo.Connect(ArmyInfo.SignalName.ClickArmy, Callable.From((string id) => EmitSignal(SignalName.ClickArmy, id)));
            politicalInfo.MoveTarget.Connect(Button.SignalName.Pressed, Callable.From(() => EmitSignal(SignalName.ClickArmyMoveTarget, province.Id)));

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

    public void CleanMoveTargets()
    {
        var politicalInfos = PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalInfo>().ToArray();
        foreach (var politicalInfo in politicalInfos)
        {
            politicalInfo.MoveTarget.Visible = false;
        }
    }

    public void ShowMoveTargets(IEnumerable<Province> provinces)
    {
        var politicalInfos = PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalInfo>()
            .Where(x => provinces.Contains(x.province))
            .ToArray();

        foreach (var politicalInfo in politicalInfos)
        {
            politicalInfo.MoveTarget.Visible = true;
        }
    }

    private void OnCentralArmyMove()
    {

    }
}
