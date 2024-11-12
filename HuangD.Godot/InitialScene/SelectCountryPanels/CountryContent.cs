using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Linq;

public partial class CountryContent : PanelContainer, IView
{
    public Label CountryName => GetNode<Label>("MarginContainer/VBoxContainer/CountryName");

    public Label ProvinceCount => GetNode<Label>("MarginContainer/VBoxContainer/ProvinceCount");

    public Label PopCount => GetNode<Label>("MarginContainer/VBoxContainer/PopCount");

    public Button Confirm => GetNode<Button>("MarginContainer/VBoxContainer/ConfirmButton");

    public Country SelectedCountry { get; private set; }

    public override void _Process(double delta)
    {
        if (this.GetSession() == null)
        {
            return;
        }

        var view = this as IView;
        if (!view.IsDirty()) { return; }

        SelectedCountry = null;

        var selectEntity = this.GetSelectEntity().Current;
        switch (selectEntity)
        {
            case Province province:
                SelectedCountry = province.Owner;
                break;
            case Country country:
                SelectedCountry = country;
                break;
            case CentralArmy centralArmy:
                SelectedCountry = centralArmy.Owner;
                break;
            default:
                break;
        }


        this.Visible = SelectedCountry != null;
        if (this.Visible)
        {
            CountryName.Text = SelectedCountry.Id;
            ProvinceCount.Text = SelectedCountry.Provinces.Count().ToString();
            PopCount.Text = SelectedCountry.PopCount.ToString();
        }
    }
}
