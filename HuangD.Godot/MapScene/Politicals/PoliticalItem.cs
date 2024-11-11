using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class PoliticalItem : Control, IView
{
    public TextureButton ProvinceButton => GetNode<TextureButton>("VBoxContainer/Province");
    public TextureButton CountryButton => GetNode<TextureButton>("VBoxContainer/Country");

    public Label ProvinceName => GetNode<Label>("VBoxContainer/Province/VBoxContainer/Name");
    public Label CountryName => GetNode<Label>("VBoxContainer/Country/Name");
    public InstancePlaceholder ArmyInfo => GetNode<InstancePlaceholder>("Armies/ArmyInfo");

    public Control BattleFlag => GetNode<Control>("VBoxContainer/Province/VBoxContainer/Battle");

    private Province GetProvince()
    {
        var politicalMap = this.GetParent<PolitcalMap>();
        var vector = politicalMap.LocalToMap(this.Position);
        GD.Print($"GetProvince {vector}");

        return this.GetSession().Provinces.Values.Single(x => x.Block.coreIndex.Equals(new HuangD.Sessions.Maps.Index(vector.X, vector.Y)));
    }

    public override void _Ready()
    {
        ProvinceButton.Connect(TextureButton.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSelectEntity().Current = GetProvince();
        }));

        CountryButton.Connect(TextureButton.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSelectEntity().Current = GetProvince().Owner;
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var province = GetProvince();
        ProvinceName.Text = province.Name;
        CountryName.Text = province.Owner.Id;

        var armyInfos = ArmyInfo.GetParent().GetChildren().OfType<ArmyInfo>();

        var needAddArmies = province.centralArmies.Except(armyInfos.Select(x => x.armyObj)).ToArray();
        var needRemoveArmies = armyInfos.Where(x => !province.centralArmies.Contains(x.armyObj)).ToArray();

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
