using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class InitialScene : Control, IView<ISessionData>
{
    public TextEdit TextEdit => GetNode<TextEdit>("CanvasLayer/VBoxContainer/BuildMapPanel/VBoxContainer/SeedEditor");
    public SelectCountryPanel SelectCountryPanel => GetNode<SelectCountryPanel>("CanvasLayer/VBoxContainer/SelectCountryPanel");

    public void Start()
    {
        var selectEntity = this.GetSession().SelectedEntity;
        if (selectEntity is not Province province)
        {
            throw new Exception();
        }

        this.GetSession().OnMessage(new Command_ChangePlayerCountry(province.Owner.Id));

        GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
    }

    public void Load()
    {
        var instance = Session.Instance;
        instance.Init(TextEdit.Text);

        this.SetSession(Decorator<ISessionData>.Create(instance));

        var commandRegister = new CommandRegister();
        GetTree().Root.AddChild(commandRegister, true);

        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate() as MapScene;
        GetTree().Root.AddChild(mapScene);
    }

    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        var selectEntity = this.GetSession().SelectedEntity;
        if (selectEntity is Province province)
        {
            var country = province.Owner;

            SelectCountryPanel.Visible = country != null;
            if (SelectCountryPanel.Visible)
            {
                SelectCountryPanel.CountryName.Text = country.Id;
                SelectCountryPanel.ProvinceCount.Text = country.Provinces.Count().ToString();
                SelectCountryPanel.PopCount.Text = country.PopCount.ToString();
            }
        }

    }
}