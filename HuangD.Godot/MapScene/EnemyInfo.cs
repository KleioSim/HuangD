﻿using Godot;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class EnemyInfo : Control
{
    public Button Button => GetNode<Button>("TextureRect/Button");
    public TextureRect ArmyIcon => GetNode<TextureRect>("TextureRect");
    public Label ArmyCount => GetNode<Label>("TextureRect/Label");
    public Label CountryName => GetNode<Label>("TextureRect/Country/Name");

    [Signal]
    public delegate void ClickArmyEventHandler(string id);

    private CentralArmy[] centralArmies;

    private int index;

    public override void _Ready()
    {
        Button.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            index++;
            if (index >= centralArmies.Length)
            {
                index = 0;
            }

            CountryName.Text = centralArmies[index].Owner.Id;

            EmitSignal(SignalName.ClickArmy, centralArmies[index].Id);
        }));
    }

    internal void Update(IEnumerable<CentralArmy> armies)
    {
        centralArmies = armies.ToArray();

        this.Visible = centralArmies.Length != 0;
        if (this.Visible)
        {
            ArmyCount.Text = "x" + armies.Count().ToString();
            CountryName.Text = centralArmies.FirstOrDefault()?.Owner.Id;
        }

    }
}
