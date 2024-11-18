using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Collections.Generic;
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
        var commandRegister = new CommandRegister();
        GetTree().Root.AddChild(commandRegister, true);

        NextTurn.Connect(
            Button.SignalName.Pressed,
            Callable.From(() => this.GetSession().OnMessage(new Command_NextTurn())));
        Army.Connect(
            Button.SignalName.Pressed,
            Callable.From(() =>
            {
                var country = this.GetSession().PlayerCountry;
                this.GetSelectEntity().Current = country.CenterArmies.OfType<Army>().Concat(country.Provinces.Select(x => x.LocalArmy));
            }));
    }

    private void OnNextTurn()
    {
        this.GetSession().OnMessage(new Command_NextTurn());
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        ArmyCount.Text = this.GetSession().PlayerCountry.CenterArmies.Sum(x => x.Count).ToString();
    }
}
