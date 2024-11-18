using Godot;
using System;

public partial class DebugFlag : Node
{
    public override void _Ready()
    {
        var console = GetTree().Root.GetNode<CommandConsole>("CommandConsole");

        var parent = GetParent() as CanvasItem;
        parent.Visible = console.IsConsoleVisable;

        console.Connect(CommandConsole.SignalName.ConsoleOpened, Callable.From(() =>
            {
                parent.Visible = true;
            }),
            (uint)ConnectFlags.ReferenceCounted);

        console.Connect(CommandConsole.SignalName.ConsoleClosed, Callable.From(() =>
            {
                parent.Visible = false;
            }),
            (uint)ConnectFlags.ReferenceCounted);
    }
}
