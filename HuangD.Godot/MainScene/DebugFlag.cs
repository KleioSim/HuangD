using Godot;
using System;

public partial class DebugFlag : Node
{
    public override void _Ready()
    {
        var parent = GetParent() as CanvasItem;
        parent.Visible = false;

        GetTree().Root.GetNode<CommandConsole>("CommandConsole")
            .Connect(CommandConsole.SignalName.ConsoleOpened, Callable.From(() =>
            {
                parent.Visible = true;
            }),
            (uint)ConnectFlags.ReferenceCounted);

        GetTree().Root.GetNode<CommandConsole>("CommandConsole")
            .Connect(CommandConsole.SignalName.ConsoleClosed, Callable.From(() =>
            {
                parent.Visible = false;
            }),
            (uint)ConnectFlags.ReferenceCounted);
    }
}
