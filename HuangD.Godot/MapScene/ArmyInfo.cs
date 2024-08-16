
using Godot;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ArmyInfo : Control
{
    public Button Button => GetNode<Button>("VBoxContainer/Country");
    public TextureRect ArmyIcon => GetNode<TextureRect>("VBoxContainer/Country");
    public Label ArmyCount => GetNode<Label>("VBoxContainer/Country");

    internal void Update(IEnumerable<CentralArmy> armies)
    {
        ArmyIcon.Visible = armies.Count() != 0;
        ArmyCount.Text = armies.Count().ToString();
    }
}
