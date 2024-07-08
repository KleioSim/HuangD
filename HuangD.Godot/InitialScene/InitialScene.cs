using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class InitialScene : Control
{
    public void Start()
    {
        GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
    }

    public void Load()
    {
        this.SetSession(new Session());

        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate();
        GetTree().Root.AddChild(mapScene);
    }
}
