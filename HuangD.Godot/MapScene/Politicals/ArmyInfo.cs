
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ArmyInfo : TextureButton, IView
{
    public Label ArmyName => GetNode<Label>("HBoxContainer/Name");
    public Label ArmyId => GetNode<Label>("HBoxContainer/Id");

    public CentralArmy armyObj
    {
        get => _armyObj;
        set
        {
            if (_armyObj == value) return;
            _armyObj = value;

            var view = this as IView;
            view.IsSelfDirty = true;
        }
    }

    private CentralArmy _armyObj;

    public override void _Ready()
    {
        this.Connect(TextureButton.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSelectEntity().Current = this.GetSession().Entities[_armyObj.Id];
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        ArmyName.Text = _armyObj.Owner.Name;
        ArmyName.Text = _armyObj.Id;
    }
}
