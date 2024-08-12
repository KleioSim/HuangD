using Chrona.Engine.Core;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class InitialScene : ViewControl
{
    public TextEdit TextEdit => GetNode<TextEdit>("CanvasLayer/VBoxContainer/BuildMapPanel/VBoxContainer/SeedEditor");

    public Label CountryName => GetNode<Label>("CanvasLayer/VBoxContainer/SelectCountryPanel/MarginContainer/VBoxContainer/CountryName");

    public Button ConfirmButton => GetNode<Button>("CanvasLayer/VBoxContainer/SelectCountryPanel/MarginContainer/VBoxContainer/ConfirmButton");

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

    }

    protected override void Update()
    {
        if (this.GetSession() == null)
        {
            return;
        }

        var playerCountry = this.GetSession().PlayerCountry;
        if (playerCountry != null)
        {
            CountryName.Text = playerCountry.Key;
            ConfirmButton.Disabled = false;
        }
        else
        {
            CountryName.Text = "";
            ConfirmButton.Disabled = true;
        }
    }
}
