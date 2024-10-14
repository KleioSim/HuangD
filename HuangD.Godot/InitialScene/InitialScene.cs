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

        mapScene.Connect(MapScene.SignalName.ClickEnity, new Callable(this, MethodName.OnSelectProvince));
    }

    public void OnSelectProvince(string id)
    {
        var province = this.GetSession().Entities[id] as Province;
        if (province != null)
        {
            this.GetSession().OnMessage(new Command_ChangePlayerCountry(province.Owner.Id));
        }
    }

    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        var playerCountry = this.GetSession()?.PlayerCountry;

        SelectCountryPanel.Visible = playerCountry != null;
        if (SelectCountryPanel.Visible)
        {
            SelectCountryPanel.CountryName.Text = playerCountry.Id;
            SelectCountryPanel.ProvinceCount.Text = playerCountry.Provinces.Count().ToString();
            SelectCountryPanel.PopCount.Text = playerCountry.PopCount.ToString();
        }
    }
}