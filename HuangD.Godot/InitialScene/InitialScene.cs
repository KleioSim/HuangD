using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class InitialScene : Control, IView
{
    public TextEdit TextEdit => GetNode<TextEdit>("CanvasLayer/VBoxContainer/BuildMapPanel/VBoxContainer/SeedEditor");
    public SelectCountryPanel SelectCountryPanel => GetNode<SelectCountryPanel>("CanvasLayer/VBoxContainer/SelectCountryPanel");

    private Country selectedCountry;

    public void Start()
    {
        this.GetSelectEntity().Current = null;

        this.GetSession().OnMessage(new Command_ChangePlayerCountry(selectedCountry.Id));

        GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
    }

    public void Load()
    {
        var instance = Session.Instance;
        instance.Init(TextEdit.Text);

        this.SetSession(Decorator.Create<ISessionData>(instance));

        var commandRegister = new CommandRegister();
        GetTree().Root.AddChild(commandRegister, true);

        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate() as MapScene;
        GetTree().Root.AddChild(mapScene);
    }

    public override void _Ready()
    {
        SelectCountryPanel.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (this.GetSession() == null)
        {
            return;
        }

        var view = this as IView;
        if (!view.IsDirty()) { return; }

        selectedCountry = null;

        var selectEntity = this.GetSelectEntity().Current;
        switch (selectEntity)
        {
            case Province province:
                selectedCountry = province.Owner;
                break;
            case Country country:
                selectedCountry = country;
                break;
            case CentralArmy centralArmy:
                selectedCountry = centralArmy.Owner;
                break;
            default:
                break;
        }


        SelectCountryPanel.Visible = selectedCountry != null;
        if (SelectCountryPanel.Visible)
        {
            SelectCountryPanel.CountryName.Text = selectedCountry.Id;
            SelectCountryPanel.ProvinceCount.Text = selectedCountry.Provinces.Count().ToString();
            SelectCountryPanel.PopCount.Text = selectedCountry.PopCount.ToString();
        }
    }
}