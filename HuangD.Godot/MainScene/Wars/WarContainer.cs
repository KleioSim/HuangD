using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;

public partial class WarContainer : ScrollContainer, IContainer, IView
{
    public InstancePlaceholder WarItem => GetNode<InstancePlaceholder>("VBoxContainer/WarItem");

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var container = this as IContainer;
        container.Refresh<WarItem>(WarItem, this.GetSession().PlayerCountry.Wars);
    }
}
