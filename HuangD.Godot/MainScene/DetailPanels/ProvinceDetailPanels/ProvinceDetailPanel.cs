using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class ProvinceDetailPanel : PanelContainer, IView
{
    public Label Title => GetNode<Label>("VBoxContainer/Header/Label");
    TabContainer TabContainer => GetNode<TabContainer>("VBoxContainer/HBoxContainer/TabContainer");

    //LocalArmyPanel LocalArmyPanel => GetNode<LocalArmyPanel>("VBoxContainer/HBoxContainer/OperationPanel/TogglePropertyPanel/VBoxContainer/LocalArmy");

    public override void _Ready()
    {
        TabContainer.Connect(TabContainer.SignalName.TabChanged, Callable.From((long index) =>
        {
            var view = this as IView;
            view.IsSelfDirty = true;
        }));

        for (int i = 0; i < TabContainer.GetTabCount(); i++)
        {
            var tabControl = TabContainer.GetTabControl(i) as ProvinceDetailTabControl;
            TabContainer.SetTabTitle(i, tabControl.TabName);
        }

        var province = this.GetSelectEntity().Current as Province;
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = this.GetSelectEntity().Current as Province;
        if(province == null)
        {
            QueueFree();
        }

        //LocalArmyPanel.armyId = province.LocalArmy.Id;

        Title.Text = province.Id;

        var control = TabContainer.GetCurrentTabControl();
        switch (control)
        {
            case ProvinceAttributeInfo attributeInfo:
                attributeInfo.Update(province);
                break;
            case ProvinceBattleInfo battleInfo:
                battleInfo.Update(province);
                break;
        }
    }
}
