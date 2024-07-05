using Godot;
using System;

public partial class InitialScene : Control
{
    public void Start()
    {
        GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
    }

    public void Load()
    {
        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate();
        GetTree().Root.AddChild(mapScene);
    }
}
