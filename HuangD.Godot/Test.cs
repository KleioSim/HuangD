using Godot;
using HuangD.Godot.Utilties;
using System;

public partial class Test : Control
{
    public override void _Ready()
    {
        CommandConsole.IsVaild = true;

        var button = GetNode<Button>("CanvasLayer/Button");
        button.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnTest), (uint)Godot.GodotObject.ConnectFlags.ReferenceCounted);
        button.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnTest1), (uint)Godot.GodotObject.ConnectFlags.ReferenceCounted);

        GD.Print($"localPositionWithOffset:{button.GetLocalPositionWithPivotOffset()}, localPosition:{button.Position}");
        GD.Print($"globalPositionWithOffset:{button.GetGlobalPositionWithPivotOffset()}, globalPosition:{button.GlobalPosition}");
    }

    private void OnTest()
    {
        GD.Print($"OnTest mouse position {GetGlobalMousePosition()}");


        //var button = GetNode<Button>("CanvasLayer/Button");
        //button.SetLocalPositionWithPivotOffset(button.GetLocalPositionWithPivotOffset() + new Vector2(10, 20));

        //GD.Print($"localPositionWithOffset:{button.GetLocalPositionWithPivotOffset()}, localPosition:{button.Position}");
        //GD.Print($"globalPositionWithOffset:{button.GetGlobalPositionWithPivotOffset()}, globalPosition:{button.GlobalPosition}");
    }

    private void OnTest1()
    {
        GD.Print("OnTest1");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventKey)
        {
            if (eventKey.Pressed)
            {
                if (eventKey.ButtonIndex == MouseButton.Left)
                {
                    GD.Print($"OnTest mouse position {GetGlobalMousePosition()}");

                    var button = GetNode<Button>("CanvasLayer/Button");
                    button.SetGlobalPositionWithPivotOffset(GetGlobalMousePosition());
                }
            }
            return;
        }
    }
}
