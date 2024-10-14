using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;

public partial class LabelTest : Label, IView<ISessionData>
{
    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }
        GD.Print("Update");

        Text = Test.data.GetName();
    }
}
