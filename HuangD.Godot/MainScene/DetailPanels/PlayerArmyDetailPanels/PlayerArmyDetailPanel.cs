using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Linq;

public partial class PlayerArmyDetailPanel : PanelContainer, IView
{
    public Label TotalArmyCount => GetNode<Label>("VBoxContainer/Title/HBoxContainer/Value");

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var playerArmyData = this.GetSelectEntity().Current as PlayerArmyData;
        if(playerArmyData == null) 
        {
            QueueFree();
            return;
        }

        TotalArmyCount.Text = playerArmyData.localArmies.Sum(x=>x.Count).ToString();
    }
}
