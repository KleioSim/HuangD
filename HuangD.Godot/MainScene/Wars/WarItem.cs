using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;

public partial class WarItem : TextureButton, IItem, IView
{
    public Label Label => GetNode<Label>("Label");

    public object Id { get; set; }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var war = Id as War;
        Label.Text = war.from != this.GetSession().PlayerCountry ? war.from.Name : war.target.Name;
    }
}