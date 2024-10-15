using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class MainScene : Control
{
    Button NextTurn => GetNode<Button>("CanvasLayer/NextTurn/Button");
    //MapScene MapScene => GetNode<MapScene>("/root/MapScene");
    InstancePlaceholder DetailPanelPlaceHolder => GetNode<InstancePlaceholder>("CanvasLayer/DetailPanelContainer");

    public override void _Ready()
    {
        NextTurn.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnNextTurn));
        //MapScene.PoliticalContainer.Connect(PoliticalContainer.SignalName.ClickArmyMoveTarget, new Callable(this, MethodName.OnStartArmyMove));
    }


    private void OnNextTurn()
    {
        this.GetSession().OnMessage(new Command_NextTurn());
    }


    //private void OnStartArmyMove(string provinceId)
    //{
    //    var detailPanel = DetailPanelPlaceHolder.GetParent().GetChildren().OfType<DetailPanelContainer>().Single();

    //    if (this.GetSession().Entities[detailPanel.EntityId] is CentralArmy centralArmy)
    //    {
    //        SendCommand(new Command_ArmyMove(detailPanel.EntityId, provinceId));

    //        MapScene.UpdateMoveInfo(detailPanel.EntityId);
    //    }
    //}
}
