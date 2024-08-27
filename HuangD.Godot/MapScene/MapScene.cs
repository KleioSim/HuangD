using DynamicData;
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
    InstancePlaceholder AmryMoveArrowPlaceHolder => GetNode<InstancePlaceholder>("CanvasLayer/ArmyMoveArrow");

    [Signal]
    public delegate void ClickEnityEventHandler(string id);

    [Signal]
    public delegate void ClickArmyMoveTargetEventHandler(string id);

    public override void _Ready()
    {
        ShowMaps();
        ShowPoliticalInfos();

        Camera.Position = TerrainMap.MapToLocal(TerrainMap.GetUsedRect().GetCenter());
    }


    internal (Vector2 position, float Rotation, float length) CalcPositionAndRotation(Province from, Province target)
    {
        var fromPos = ProvinceMap.GetPawnLocation(from.Id);
        var targetPos = ProvinceMap.GetPawnLocation(target.Id);

        var position = targetPos;
        var angle = (float)(Math.Atan2((targetPos.Y - fromPos.Y), (targetPos.X - fromPos.X)) * 180 / Math.PI)+90;
        var length = fromPos.DistanceTo(targetPos);

        GD.Print($"from:{fromPos} target:{targetPos} angle:{angle}");
        return (position, angle, length);
    }

    private void ShowPoliticalInfos()
    {
        var session = this.GetSession();
        foreach (var province in session.Entities.Values.OfType<Province>())
        {
            var politicalInfo = PoliticalInfoPlaceHolder.CreateInstance() as PoliticalInfo;
            politicalInfo.Position = ProvinceMap.GetPawnLocation(province.Id);
            politicalInfo.province = province;
            politicalInfo.ArmyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.MoveTarget.Connect(Button.SignalName.Pressed, Callable.From(() => EmitSignal(SignalName.ClickArmyMoveTarget, province.Id)));
            politicalInfo.MoveTarget.Visible = false;

            politicalInfo.OnZoomed(Camera.Zoom);

            Camera.Connect(MapCamera2D.SignalName.OnZoomed, new Callable(politicalInfo, PoliticalInfo.MethodName.OnZoomed));
        }
    }

    private void ShowMaps()
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
                        OnClickEntity(provinceId);
                        GD.Print(provinceId);
                    }
                }
            }
            return;
        }
    }

    private void OnClickEntity(string id)
    {
        UpdateMoveInfo(id);

        EmitSignal(SignalName.ClickEnity, id);
    }

    public void UpdateMoveInfo(string id)
    {
        UpdateMoveTarget(id);
        UpdateMoveArrow(id);
    }

    private void UpdateMoveArrow(string id)
    {
        var amryMoveArrows = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().ToList();
        var army = this.GetSession().Entities[id] as CentralArmy;
        foreach (var arrow in amryMoveArrows.ToArray())
        {
            if (army == null
                || army.MoveTo == null
                || arrow.ArmyId != army.Id)
            {
                arrow.QueueFree();
                amryMoveArrows.Remove(arrow);
            }
        }

        if (amryMoveArrows.Count == 0
            && army != null
            && army.MoveTo != null)
        {
            var currArmyArrow = AmryMoveArrowPlaceHolder.CreateInstance() as AmryMoveArrow;
            currArmyArrow.MapScene = this;
            currArmyArrow.ArmyId = army.Id;
        }

    }

    private void UpdateMoveTarget(string id)
    {
        var targetProvince = Enumerable.Empty<Province>();
        if (this.GetSession().Entities[id] is CentralArmy centralArmy)
        {
            if (centralArmy.MoveTo == null)
            {
                targetProvince = centralArmy.Position.Neighbors;
            }
        }

        var politicalInfos = PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalInfo>().ToArray();
        foreach (var politicalInfo in politicalInfos)
        {
            politicalInfo.MoveTarget.Visible = targetProvince.Contains(politicalInfo.province);
        }
    }
}
