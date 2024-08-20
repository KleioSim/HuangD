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

        ViewControl.OnMessage = this.GetSession().OnMessage;

        var commandRegister = new CommandRegister();
        commandRegister.SendCommand = ViewControl.SendCommand;

        GetTree().Root.AddChild(commandRegister, true);

        var mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate() as MapScene;
        GetTree().Root.AddChild(mapScene);

        mapScene.Connect(MapScene.SignalName.ClickProvince, new Callable(this, MethodName.OnSelectProvince));

        //CommandConsole.AddCommand("TestInt", TestInt);
    }

    public void OnSelectProvince(string provinceId)
    {
        var province = this.GetSession().Provinces[provinceId];

        SendCommand(new Command_ChangePlayerCountry(province.Owner.Id));
    }

    protected override void Initialize()
    {

    }

    protected override void Update()
    {
        var playerCountry = this.GetSession()?.PlayerCountry;

        SelectCountryPanel.Visible = playerCountry != null;
        if (SelectCountryPanel.Visible)
        {
            SelectCountryPanel.CountryName.Text = playerCountry.Id;
            SelectCountryPanel.ProvinceCount.Text = playerCountry.Provinces.Count().ToString();
            SelectCountryPanel.PopCount.Text = playerCountry.PopCount.ToString();
        }
    }

    void TestInt(int id)
    {
        GD.Print(id + 1);
    }
}