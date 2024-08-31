using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;

public partial class BattleInfo : Control
{
    internal void Update(Battle battle)
    {
        this.Visible = battle != null;
    }
}