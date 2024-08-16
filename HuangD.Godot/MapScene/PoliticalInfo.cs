using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class PoliticalInfo : ViewControl
{
    public Label ProvinceName => GetNode<Label>("VBoxContainer/Province/Name");
    public Label CountryName => GetNode<Label>("VBoxContainer/Country/Name");
    public TextureRect CountryFlag => GetNode<TextureRect>("VBoxContainer/Country");

    public ArmyInfo ArmyInfo => GetNode<ArmyInfo>("VBoxContainer/Army");
    public ArmyInfo EnemyInfo => GetNode<ArmyInfo>("VBoxContainer/Army");

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

        CountryFlag.SelfModulate = Color.FromHsv(_province.Owner.Color.h, _province.Owner.Color.s, _province.Owner.Color.v);

        ArmyInfo.Update(_province.centralArmies.Where(x => x.Owner == _province.Owner));
        EnemyInfo.Update(_province.centralArmies.Where(x => x.Owner != _province.Owner));
    }
}
