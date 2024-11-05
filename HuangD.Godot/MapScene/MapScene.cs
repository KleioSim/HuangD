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

public partial class MapScene : GraphEdit
{
    GraphElement GraphElement => GetNode<GraphElement>("GraphElement");
    BaseMap BaseMap => GetNode<BaseMap>("GraphElement/BaseMap");
    PoliticalContainer PoliticalContainer => GetNode<PoliticalContainer>("GraphElement/MarginContainer/PoliticalContainer");

    public override void _Ready()
    {
        GraphElement.Size = BaseMap.GetSize();

        PoliticalContainer.BuildPoliticalInfos(BaseMap.GetProvinceCenter);
    }

    //public override void _UnhandledInput(InputEvent @event)
    //{
    //    if (@event is InputEventMouseButton eventKey)
    //    {
    //        if (eventKey.Pressed)
    //        {
    //            if (eventKey.ButtonIndex == MouseButton.Left)
    //            {
    //                var provinceId = BaseMap.LocalToProvince(GetGlobalMousePosition());

    //                if (provinceId != null)
    //                {
    //                    this.GetSelectEntity().Current = this.GetSession().Entities[provinceId];
    //                }
    //            }
    //        }
    //        return;
    //    }
    //}
}
