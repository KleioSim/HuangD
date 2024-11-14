using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class ProvinceDetailPanel : PanelContainer, IView
{
    public Label Title => GetNode<Label>("VBoxContainer/Title/HBoxContainer/Label");
    //TabContainer TabContainer => GetNode<TabContainer>("VBoxContainer/HBoxContainer/TabContainer");

    //LocalArmyPanel LocalArmyPanel => GetNode<LocalArmyPanel>("VBoxContainer/HBoxContainer/OperationPanel/TogglePropertyPanel/VBoxContainer/LocalArmy");



    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = this.GetSelectEntity().Current as Province;
        if (province == null)
        {
            QueueFree();
            return;
        }

        //LocalArmyPanel.armyId = province.LocalArmy.Id;

        Title.Text = province.Name;

        //var control = TabContainer.GetCurrentTabControl();
        //switch (control)
        //{
        //    case ProvinceAttributeInfo attributeInfo:
        //        attributeInfo.Update(province);
        //        break;
        //    case ProvinceBattleInfo battleInfo:
        //        battleInfo.Update(province);
        //        break;
        //}
    }
}
