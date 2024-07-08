using Godot;
using System;

public partial class Test : Control
{
    public override void _Ready()
    {
        var button = GetNode<Button>("Button");
        button.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnTest), (uint)Godot.GodotObject.ConnectFlags.ReferenceCounted);
        button.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnTest1), (uint)Godot.GodotObject.ConnectFlags.ReferenceCounted);
    }

    private void OnTest()
    {
        GD.Print("OnTest");
    }

    private void OnTest1()
    {
        GD.Print("OnTest1");
    }
}
