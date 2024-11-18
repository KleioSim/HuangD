using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;

public partial class WarItem : TextureButton, IItem, IView
{
    public Label CountryName => GetNode<Label>("HBoxContainer/CountryName");
    public Label CountryId => GetNode<Label>("HBoxContainer/CountryId");

    public object Id { get; set; }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var war = Id as War;

        var peer = war.from != this.GetSession().PlayerCountry ? war.from : war.target;
        CountryName.Text = peer.Name;
        CountryId.Text = peer.Id;
    }
}