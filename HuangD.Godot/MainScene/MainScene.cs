using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class MainScene : Control, IView
{
    Button NextTurn => GetNode<Button>("CanvasLayer/NextTurn/Button");

    Button Army => GetNode<Button>("CanvasLayer/TopInfos/Army/Button");
    Label ArmyCount => GetNode<Label>("CanvasLayer/TopInfos/Army/HBoxContainer/Value");

    //MapScene MapScene => GetNode<MapScene>("/root/MapScene");
    InstancePlaceholder DetailPanelPlaceHolder => GetNode<InstancePlaceholder>("CanvasLayer/DetailPanelContainer");

    public override void _Ready()
    {
        NextTurn.Connect(
            Button.SignalName.Pressed, 
            Callable.From(()=> this.GetSession().OnMessage(new Command_NextTurn())));
        Army.Connect(
            Button.SignalName.Pressed,
            Callable.From(() => this.GetSession().OnMessage(new Command_SelectEntity("PlayerArmy"))));
    }


    private void OnNextTurn()
    {
        this.GetSession().OnMessage(new Command_NextTurn());
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        ArmyCount.Text = this.GetSession().PlayerCountry.Provinces.Sum(x=>x.LocalArmy.Count).ToString();
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
