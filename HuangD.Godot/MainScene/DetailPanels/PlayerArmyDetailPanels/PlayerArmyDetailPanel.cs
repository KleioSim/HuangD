using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerArmyDetailPanel : PanelContainer, IView
{
    public Label CentralCurrent => GetNode<Label>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Current");
    public Label CentralExpect => GetNode<Label>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Expect");
    public Label CentralIncrease => GetNode<Label>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Increase");
    public Label CentralSpend => GetNode<Label>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Spend");

    public Label LocalCurrent => GetNode<Label>("VBoxContainer/TabContainer/LocalArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Current");
    public Label LocalExpect => GetNode<Label>("VBoxContainer/TabContainer/LocalArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Expect");
    public Label LocalIncrease => GetNode<Label>("VBoxContainer/TabContainer/LocalArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Increase");
    public Label LocalSpend => GetNode<Label>("VBoxContainer/TabContainer/LocalArmies/VBoxContainer/Total/HBoxContainer/VBoxContainer/Datas/Spend");

    public InstancePlaceholder CentralArmyItem => GetNode<InstancePlaceholder>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/ScrollContainer/Content/ArmyItem");
    public InstancePlaceholder LcoalArmyItem => GetNode<InstancePlaceholder>("VBoxContainer/TabContainer/LocalArmies/VBoxContainer/ScrollContainer/Content/ArmyItem");

    public Button AddCentral => GetNode<Button>("VBoxContainer/TabContainer/CenterArmies/VBoxContainer/HBoxContainer/Add");

    public override void _Ready()
    {
        AddCentral.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            var armies = this.GetSelectEntity().Current as IEnumerable<Army>;
            if (armies == null)
            {
                return;
            }

            this.GetSession().OnMessage(new Command_CreateCenterlArmy(armies.First().Owner.Id, (int)ArmyLevel.MID));
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var armies = this.GetSelectEntity().Current as IEnumerable<Army>;
        if (armies == null)
        {
            QueueFree();
            return;
        }

        var centralArmies = armies.OfType<CentralArmy>();
        CentralCurrent.Text = centralArmies.Sum(x => x.Count).ToString();
        CentralExpect.Text = centralArmies.Sum(x => x.ExpectCount).ToString();
        CentralIncrease.Text = centralArmies.Sum(x => x.IncCount).ToString();
        CentralSpend.Text = centralArmies.Sum(x => x.Cost).ToString();

        var items = CentralArmyItem.GetParent().GetChildren().OfType<ArmyItem>().ToArray();
        foreach (var item in items.Where(x => !centralArmies.Contains(x.ArmyObj)).ToArray())
        {
            item.QueueFree();
        }
        foreach (var army in centralArmies.Except(items.Select(x => x.ArmyObj)).ToArray())
        {
            var newItem = CentralArmyItem.CreateInstance() as ArmyItem;
            newItem.ArmyObj = army;
        }

        var localArmies = armies.OfType<LocalArmy>();
        LocalCurrent.Text = localArmies.Sum(x => x.Count).ToString();
        LocalExpect.Text = localArmies.Sum(x => x.ExpectCount).ToString();
        LocalIncrease.Text = localArmies.Sum(x => x.IncCount).ToString();
        LocalSpend.Text = localArmies.Sum(x => x.Cost).ToString();

        items = LcoalArmyItem.GetParent().GetChildren().OfType<ArmyItem>().ToArray();
        foreach (var item in items.Where(x => !localArmies.Contains(x.ArmyObj)).ToArray())
        {
            item.QueueFree();
        }
        foreach (var army in localArmies.Except(items.Select(x => x.ArmyObj)).ToArray())
        {
            var newItem = LcoalArmyItem.CreateInstance() as ArmyItem;
            newItem.ArmyObj = army;
        }
    }

}
