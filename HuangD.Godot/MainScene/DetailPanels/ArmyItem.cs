using Chrona.Engine.Godot;
using Godot;
using Godot.Collections;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class ArmyItem : Control, IView
{
    public Army ArmyObj { get; set; }

    public Label Current => GetNode<Label>("Datas/Current");
    public Label Expect => GetNode<Label>("Datas/Expect");
    public Label Increase => GetNode<Label>("Datas/Increase");
    public Label Spend => GetNode<Label>("Datas/Spend");
    public OptionButton Level => GetNode<OptionButton>("Datas/Control/Level");
    public Button Disband => GetNode<Button>("Disband");

    public override void _Ready()
    {
        Level.Connect(OptionButton.SignalName.ItemSelected, Callable.From((int idx) =>
        {
            this.GetSession().OnMessage(new Command_ChangeArmyLevel(ArmyObj.Id, idx));
        }));

        Disband.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSession().OnMessage(new Command_DisbandCentralArmy(ArmyObj.Id));
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        Current.Text = ArmyObj.Count.ToString();
        Expect.Text = ArmyObj.ExpectCount.ToString();
        Increase.Text = ArmyObj.IncCount.ToString();
        Spend.Text = ArmyObj.Cost.ToString();

        Level.Selected = (int)ArmyObj.Level;

        Disband.Disabled = !(ArmyObj is CentralArmy);
    }
}
