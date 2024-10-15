using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class PoliticalItem : Control, IView<ISessionData>
{
    public Label ProvinceName => GetNode<Label>("HBoxContainer/VBoxContainer/Province/Panel/Name");
    public Label CountryName => GetNode<Label>("HBoxContainer/VBoxContainer/Country/Name");
    public Control CurrentOwner => GetNode<Control>("HBoxContainer");

    public ArmyInfo ArmyInfo => GetNode<ArmyInfo>("HBoxContainer/Military/HBoxContainer/Army");
    public EnemyInfo EnemyInfo => GetNode<EnemyInfo>("HBoxContainer/Military/HBoxContainer/Enemy");

    public BattleInfo BattleInfo => GetNode<BattleInfo>("HBoxContainer/Military/HBoxContainer/Enemy/Battle");
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

    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        ProvinceName.Text = _province.Id;
        CountryName.Text = _province.Owner.Id;

        //CurrentOwner.Modulate = Color.FromHsv(_province.Owner.Color.h, _province.Owner.Color.s, _province.Owner.Color.v);

        ArmyInfo.Update(_province.centralArmies.Where(x => x.Owner == _province.Owner));
        EnemyInfo.Update(_province.centralArmies.Where(x => x.Owner != _province.Owner));
        BattleInfo.Update(_province.Battle);

        var selectedEntity = this.GetSession().SelectedEntity;
        MoveTarget.Visible = (selectedEntity is Army) && ((Army)selectedEntity).Position.Neighbors.Contains(_province);
    }
}
