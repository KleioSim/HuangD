using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;

public partial class PoliticalInfo : Control
{
    public Label ProvinceName => GetNode<Label>("VBoxContainer/ProvinceName");
    public Label CountryName => GetNode<Label>("VBoxContainer/CountryName");

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
}
