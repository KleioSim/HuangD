using Chrona.Engine.Godot.Utilties;
using Godot;
using System;

namespace HuangD.Godot.Utilties;

static class IDisposableExtentions
{
    public static void EndWith(this IDisposable disposable, Node node, StringName signalName)
    {
        node.Connect(signalName,
            Callable.From(() =>
            {
                disposable.Dispose();
            }),
            (uint)GodotObject.ConnectFlags.ReferenceCounted);
    }
}
