using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

public partial class MapScene : Node2D
{
    BaseMap BaseMap => GetNode<BaseMap>("CanvasLayer/BaseMap");

    MapCamera2D Camera => GetNode<MapCamera2D>("CanvasLayer/Camera2D");

    PoliticalContainer PoliticalContainer => GetNode<PoliticalContainer>("CanvasLayer/PoliticalContainer");
    ArrowContainer ArrowContainer => GetNode<ArrowContainer>("CanvasLayer/ArrowContainer");

    //[Signal]
    //public delegate void ClickEnityEventHandler(string id);

    public override void _Ready()
    {
        AmryMoveArrow.GetPoliticalItem = PoliticalContainer.GetItem;

        Camera.Connect(MapCamera2D.SignalName.OnZoomed, Callable.From<Vector2>((vector) =>
        {
            PoliticalContainer.OnCameraZoom(vector);
            ArrowContainer.OnCameraZoom(vector);
        }));

        Camera.Position = BaseMap.GetMapCenter();

        PoliticalContainer.BuildPoliticalInfos(BaseMap.GetProvinceCenter);
        PoliticalContainer.OnCameraZoom(Camera.Zoom);
    }

    //private void OnCameraZoom(Vector2 zoom)
    //{
    //    foreach (var politicalInfo in politicalInfos.Values)
    //    {
    //        politicalInfo.OnZoomed(zoom);
    //    }

    //    var amryMoveArrows = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().ToList();
    //    foreach (var arrow in amryMoveArrows)
    //    {
    //        arrow.OnZoom();
    //    }
    //}

    //internal (Vector2 position, float Rotation, float length) CalcPositionAndRotation(Province from, Province target)
    //{

    //    var fromPolitical = politicalInfos[from.Id];
    //    var targetPolitical = politicalInfos[target.Id];

    //    var fromPos = fromPolitical.ArmyInfo.ArmyIcon.GetGlobalPositionWithPivotOffset();
    //    var targetPos = targetPolitical.MoveTarget.GetGlobalPositionWithPivotOffset();

    //    var position = targetPos;
    //    var angle = (float)(Math.Atan2((targetPos.Y - fromPos.Y), (targetPos.X - fromPos.X)) * 180 / Math.PI) + 90;
    //    var length = fromPos.DistanceTo(targetPos);

    //    GD.Print($"targetPolitical.MoveTarget position:{targetPolitical.MoveTarget.GetGlobalPositionWithPivotOffset()}");

    //    return (position, angle, length);
    //}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventKey)
        {
            if (eventKey.Pressed)
            {
                if (eventKey.ButtonIndex == MouseButton.Left)
                {
                    var provinceId = BaseMap.LocalToProvince(GetGlobalMousePosition());

                    if (provinceId != null)
                    {
                        this.GetSelectEntity().Current = this.GetSession().Entities[provinceId];
                    }
                }
            }
            return;
        }
    }

    //public void UpdateMoveInfo(string id)
    //{
    //    //UpdateMoveTarget(id);
    //    UpdateMoveArrow(id);
    //}

    //private void UpdateMoveArrow(string id)
    //{
    //    var amryMoveArrows = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().ToList();
    //    var army = this.GetSession().Entities[id] as CentralArmy;
    //    foreach (var arrow in amryMoveArrows.ToArray())
    //    {
    //        if (army == null
    //            || army.MoveTo == null
    //            || arrow.ArmyId != army.Id)
    //        {
    //            arrow.QueueFree();
    //            amryMoveArrows.Remove(arrow);
    //        }
    //    }

    //    if (amryMoveArrows.Count == 0
    //        && army != null
    //        && army.MoveTo != null)
    //    {
    //        var currArmyArrow = AmryMoveArrowPlaceHolder.CreateInstance() as AmryMoveArrow;
    //        currArmyArrow.MapScene = this;
    //        currArmyArrow.ArmyId = army.Id;
    //    }

    //}

    //private void UpdateMoveTarget(string id)
    //{
    //    var targetProvince = Enumerable.Empty<Province>();
    //    if (this.GetSession().Entities[id] is CentralArmy centralArmy)
    //    {
    //        if (centralArmy.MoveTo == null)
    //        {
    //            targetProvince = centralArmy.Position.Neighbors;
    //        }
    //    }

    //    foreach (var politicalInfo in politicalInfos.Values)
    //    {
    //        politicalInfo.MoveTarget.Visible = targetProvince.Contains(politicalInfo.province);
    //    }
    //}
}
