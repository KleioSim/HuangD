using Godot;
using HuangD.Sessions;
using System;

public partial class LabelTest : Label, IView
{
    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }
        GD.Print("Update");

        Text = Test.data.GetName();
    }
}
