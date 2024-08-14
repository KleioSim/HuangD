using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;

public partial class PoliticalInfo : ViewControl
{
    public Label ProvinceName => GetNode<Label>("VBoxContainer/Province/Name");
    public Label CountryName => GetNode<Label>("VBoxContainer/Country/Name");

    public Province province
    {
        get
        {
            return _province;
        }
        set
        {
            _province = value;
            ProvinceName.Text = _province.Key;
            CountryName.Text = _province.Owner.Key;
        }
    }

    private Province _province;
    public void OnZoomed(Vector2 zoom)
    {
        this.Scale = Vector2.One / zoom;
    }

    protected override void Initialize()
    {

    }

    protected override void Update()
    {
        ProvinceName.Text = _province.Key;
        CountryName.Text = _province.Owner.Key;

        var styleBox = GetThemeStylebox("panel");
    }
}
