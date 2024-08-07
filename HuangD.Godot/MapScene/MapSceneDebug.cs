using Godot;
using HuangD.Godot.Utilties;
using System;

public partial class MapSceneDebug : Node2D
{
    public override void _Ready()
    {
        this.SetSession(new HuangD.Sessions.Session("Test"));
    }
}
