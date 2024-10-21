using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class LocalArmyPanel : PanelContainer,IView
{
    internal string armyId;

    public Label localArmyCount => GetNode<Label>("HBoxContainer/Title/HBoxContainer/Count");

    public Button VLow => GetNode<Button>("HBoxContainer/VBoxContainer/Levels/VLow");
    public Button Low => GetNode<Button>("HBoxContainer/VBoxContainer/Levels/Low");
    public Button Mid => GetNode<Button>("HBoxContainer/VBoxContainer/Levels/Mid");
    public Button High => GetNode<Button>("HBoxContainer/VBoxContainer/Levels/High");
    public Button VHigh => GetNode<Button>("HBoxContainer/VBoxContainer/Levels/VHigh");

    public override void _Ready()
    {
        VLow.Connect(Button.SignalName.Pressed, Callable.From(() => ChangeArmyLevel(ArmyLevel.VeryLow)));
        Low.Connect(Button.SignalName.Pressed, Callable.From(() => ChangeArmyLevel(ArmyLevel.Low)));
        Mid.Connect(Button.SignalName.Pressed, Callable.From(() => ChangeArmyLevel(ArmyLevel.Mid)));
        High.Connect(Button.SignalName.Pressed, Callable.From(() => ChangeArmyLevel(ArmyLevel.High)));
        VHigh.Connect(Button.SignalName.Pressed, Callable.From(() => ChangeArmyLevel(ArmyLevel.VeryHigh)));
    }

    private void ChangeArmyLevel(ArmyLevel level)
    {
        this.GetSession().OnMessage(new Command_ChangLocalArmyLevel(armyId, (int)level));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = this.GetSelectEntity().Current as Province;
        if (province == null)
        {
            return;
        }

        var army = province.LocalArmy;
        armyId = army.Id;

        localArmyCount.Text = army.Count.ToString();

        switch (army.Level)
        {
            case ArmyLevel.VeryLow:
                VLow.SetPressedNoSignal(true);
                break;
            case ArmyLevel.Low:
                Low.SetPressedNoSignal(true);
                break;
            case ArmyLevel.Mid:
                Mid.SetPressedNoSignal(true);
                break;
            case ArmyLevel.High:
                High.SetPressedNoSignal(true);
                break;
            case ArmyLevel.VeryHigh:
                VHigh.SetPressedNoSignal(true);
                break;
            default: 
                throw new NotImplementedException();
        }
    }

}
