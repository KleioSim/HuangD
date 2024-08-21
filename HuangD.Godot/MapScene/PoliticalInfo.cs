using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class PoliticalInfo : ViewControl
{
    public Label ProvinceName => GetNode<Label>("HBoxContainer/VBoxContainer/Province/Panel/Name");
    public Label CountryName => GetNode<Label>("HBoxContainer/VBoxContainer/Country/Name");
    public Control CurrentOwner => GetNode<Control>("HBoxContainer");

    public ArmyInfo ArmyInfo => GetNode<ArmyInfo>("HBoxContainer/Army");
    public ArmyInfo EnemyInfo => GetNode<ArmyInfo>("VBoxContainer/Army");

    public MoveTarget MoveTarget => GetNode<MoveTarget>("HBoxContainer/VBoxContainer/Province/MoveTarget");

    public Province province
    {
        get
        {
            return _province;
        }
        set
        {
            _province = value;
            ProvinceName.Text = _province.Id;
            CountryName.Text = _province.Owner.Id;
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
        ProvinceName.Text = _province.Id;
        CountryName.Text = _province.Owner.Id;

        CurrentOwner.Modulate = Color.FromHsv(_province.Owner.Color.h, _province.Owner.Color.s, _province.Owner.Color.v);

        ArmyInfo.Update(_province.centralArmies.Where(x => x.Owner == _province.Owner));
        //EnemyInfo.Update(_province.centralArmies.Where(x => x.Owner != _province.Owner));
    }
}
