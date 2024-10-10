using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PoliticalContainer : Control
{
    [Signal]
    public delegate void ClickArmyMoveTargetEventHandler(string id);

    [Signal]
    public delegate void ClickEnityEventHandler(string id);

    InstancePlaceholder PoliticalInfoPlaceHolder => GetNode<InstancePlaceholder>("Item");

    public override void _Ready()
    {

    }

    public void BuildPoliticalInfos(Func<string, Vector2> FuncGetProvinceCenter)
    {
        var session = this.GetSession();

        foreach (var province in session.Entities.Values.OfType<Province>())
        {
            var politicalInfo = PoliticalInfoPlaceHolder.CreateInstance() as PoliticalItem;
            politicalInfo.Name = province.Id;

            politicalInfo.Position = FuncGetProvinceCenter(province.Id);
            politicalInfo.province = province;
            politicalInfo.ArmyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.EnemyInfo.Connect(ArmyInfo.SignalName.ClickArmy, new Callable(this, MethodName.OnClickEntity));
            politicalInfo.MoveTarget.Connect(Button.SignalName.ButtonDown, Callable.From(() =>
            {
                GD.Print($"mouse {GetGlobalMousePosition()}");
                GD.Print($"politicalInfo.MoveTarget  GlobalPositionPosition:{politicalInfo.MoveTarget.GlobalPosition}");
                GD.Print($"politicalInfo.MoveTarget  GlobalPositionPositionWithOffset:{politicalInfo.MoveTarget.GetGlobalPositionWithPivotOffset()}");
                EmitSignal(SignalName.ClickArmyMoveTarget, province.Id);
            }));
            politicalInfo.MoveTarget.Visible = false;
        }
    }

    internal void OnCameraZoom(Vector2 zoom)
    {
        foreach (var item in PoliticalInfoPlaceHolder.GetParent().GetChildren().OfType<PoliticalItem>())
        {
            item.OnZoomed(zoom);
        }
    }

    private void OnClickEntity(string id)
    {
        //UpdateMoveInfo(id);

        EmitSignal(SignalName.ClickEnity, id);
    }
}
