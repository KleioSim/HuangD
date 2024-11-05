using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class PoliticalItem : HBoxContainer, IView
{
    public TextureButton ProvinceButton => GetNode<TextureButton>("VBoxContainer/Province");
    public TextureButton CountryButton => GetNode<TextureButton>("VBoxContainer/Country");

    public Label ProvinceName => GetNode<Label>("VBoxContainer/Province/VBoxContainer/Name");
    public Label CountryName => GetNode<Label>("VBoxContainer/Country/Name");

    //public Control CurrentOwner => GetNode<Control>("HBoxContainer");
    public InstancePlaceholder ArmyInfo => GetNode<InstancePlaceholder>("Armies/ArmyInfo");

    public Control BattleFlag => GetNode<Control>("VBoxContainer/Province/VBoxContainer/Battle");

    //public ArmyInfo ArmyInfo => GetNode<ArmyInfo>("HBoxContainer/Military/HBoxContainer/Army");
    //public EnemyInfo EnemyInfo => GetNode<EnemyInfo>("HBoxContainer/Military/HBoxContainer/Enemy");

    //public BattleInfo BattleInfo => GetNode<BattleInfo>("HBoxContainer/Military/HBoxContainer/Enemy/Battle");
    //public MoveTarget MoveTarget => GetNode<MoveTarget>("HBoxContainer/VBoxContainer/Province/MoveTarget");

    public Province province
    {
        get
        {
            return _province;
        }
        set
        {
            if (_province == value)
            {
                return;
            }

            _province = value;
            var view = this as IView;
            view.IsSelfDirty = true;
        }
    }

    private Province _province;


    public override void _Ready()
    {
        ProvinceButton.Connect(TextureButton.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSelectEntity().Current = this.GetSession().Entities[_province.Id];
        }));

        CountryButton.Connect(TextureButton.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSelectEntity().Current = this.GetSession().Entities[_province.Owner.Id];
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        ProvinceName.Text = _province.Name;
        CountryName.Text = _province.Owner.Id;

        var armyInfos = ArmyInfo.GetParent().GetChildren().OfType<ArmyInfo>();

        var needAddArmies = _province.centralArmies.Except(armyInfos.Select(x => x.armyObj)).ToArray();
        var needRemoveArmies = armyInfos.Where(x => !_province.centralArmies.Contains(x.armyObj)).ToArray();

        foreach (var item in needRemoveArmies)
        {
            item.QueueFree();
        }

        foreach (var item in needAddArmies)
        {
            var armyInfo = ArmyInfo.CreateInstance() as ArmyInfo;
            armyInfo.armyObj = item;
        }

        BattleFlag.Visible = false;

        //CurrentOwner.Modulate = Color.FromHsv(_province.Owner.Color.h, _province.Owner.Color.s, _province.Owner.Color.v);

        //ArmyInfo.Update(_province.centralArmies.Where(x => x.Owner == _province.Owner));
        //EnemyInfo.Update(_province.centralArmies.Where(x => x.Owner != _province.Owner));
        //BattleInfo.Update(_province.Battle);

        //var selectedEntity = this.GetSelectEntity().Current;
        //MoveTarget.Visible = (selectedEntity is Army) && ((Army)selectedEntity).Position.Neighbors.Contains(_province);
    }
}
