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

    private Dictionary<string, PoliticalInfo> politicalInfos;

    public override void _Ready()
    {
        ShowMaps();
        politicalInfos = ShowPoliticalInfos().ToDictionary(x => x.province.Id, y => y);

        Camera.Position = TerrainMap.MapToLocal(TerrainMap.GetUsedRect().GetCenter());

        Camera.Connect(MapCamera2D.SignalName.OnZoomed, Callable.From<Vector2>((zoom) =>
        {
            foreach (var politicalInfo in politicalInfos.Values)
            {
                politicalInfo.OnZoomed(zoom);
            }

            var amryMoveArrows = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().ToList();
            foreach (var arrow in amryMoveArrows)
            {
                arrow.OnZoom();
            }

        }));
    }


    internal (Vector2 position, float Rotation, float length) CalcPositionAndRotation(Province from, Province target)
    {

        var fromPolitical = politicalInfos[from.Id];
        var targetPolitical = politicalInfos[target.Id];

        var fromPos = fromPolitical.ArmyInfo.ArmyIcon.GlobalPosition + (fromPolitical.ArmyInfo.ArmyIcon.Size / 2) / Camera.Zoom;
        var targetPos = targetPolitical.MoveTarget.GlobalPosition + (targetPolitical.MoveTarget.Size / 2) / Camera.Zoom;

        var position = targetPos;
        var angle = (float)(Math.Atan2((targetPos.Y - fromPos.Y), (targetPos.X - fromPos.X)) * 180 / Math.PI) + 90;
        var length = fromPos.DistanceTo(targetPos);

        return (position, angle, length);
    }

    private IEnumerable<PoliticalInfo> ShowPoliticalInfos()
    {
        var list = new List<PoliticalInfo>();
        var session = this.GetSession();
        foreach (var province in session.Entities.Values.OfType<Province>())
        {
            var politicalInfo = PoliticalInfoPlaceHolder.CreateInstance() as PoliticalInfo;
            list.Add(politicalInfo);

            politicalInfo.Position = ProvinceMap.GetPawnLocation(province.Id);
            politicalInfo.province = province;
            politicalInfo.ArmyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.MoveTarget.Connect(Button.SignalName.ButtonDown, Callable.From(() => EmitSignal(SignalName.ClickArmyMoveTarget, province.Id)));
            politicalInfo.MoveTarget.Visible = false;

            politicalInfo.OnZoomed(Camera.Zoom);
        }

        return list;
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

        foreach (var politicalInfo in politicalInfos.Values)
        {
            politicalInfo.MoveTarget.Visible = targetProvince.Contains(politicalInfo.province);
        }
    }
}
