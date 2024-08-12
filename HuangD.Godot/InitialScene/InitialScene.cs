using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class InitialScene : ViewControl
{
    public TextEdit TextEdit => GetNode<TextEdit>("CanvasLayer/VBoxContainer/BuildMapPanel/VBoxContainer/SeedEditor");
    public SelectCountryPanel SelectCountryPanel => GetNode<SelectCountryPanel>("CanvasLayer/VBoxContainer/SelectCountryPanel");

    public void Start()
    {
        GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
    }

    public void Load()
    {
        this.SetSession(new Session(TextEdit.Text));

        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate() as MapScene;
        GetTree().Root.AddChild(mapScene);

        mapScene.Connect(MapScene.SignalName.ClickProvince, new Callable(this, MethodName.OnSelectProvince));
    }

    public void OnSelectProvince(string provinceId)
    {
        var province = this.GetSession().Provinces[provinceId];

        SendCommand(new Command_ChangePlayerCountry(province.Owner.Key));
    }

    protected override void Initialize()
    {
        ViewControl.OnMessage = (msg) => this.GetSession().OnMessage(msg);
    }

    protected override void Update()
    {
        SelectCountryPanel.Visible = this.GetSession() != null;
        if (SelectCountryPanel.Visible)
        {
            var playerCountry = this.GetSession().PlayerCountry;
            SelectCountryPanel.CountryName.Text = playerCountry.Key;
            SelectCountryPanel.ProvinceCount.Text = playerCountry.Provinces.Count().ToString();
            SelectCountryPanel.PopCount.Text = playerCountry.PopCount.ToString();
        }
    }
}
