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
    public SeedPanel SeedPanel => GetNode<SeedPanel>("CanvasLayer/SeedPanel");
    public SelectCountryPanel SelectCountryPanel => GetNode<SelectCountryPanel>("CanvasLayer/SelectCountryPanel");

    private MapScene mapScene;

    public override void _Ready()
    {
        SelectCountryPanel.Visible = false;

        SeedPanel.Connect(SeedPanel.SignalName.Confirm, Callable.From((string seed) =>
        {
            var instance = Session.Instance;
            instance.Init(seed);

            this.SetSession(Decorator.Create<ISessionData>(instance));

            mapScene = ResourceLoader.Load<PackedScene>("res://MapScene/MapScene.tscn").Instantiate() as MapScene;
            GetTree().Root.AddChild(mapScene);

            SeedPanel.Visible = false;
            SelectCountryPanel.Visible = true;
        }));

        SelectCountryPanel.Connect(SelectCountryPanel.SignalName.Next, Callable.From((string id) =>
        {
            this.GetSelectEntity().Current = null;

            this.GetSession().OnMessage(new Command_ChangePlayerCountry(id));

            GetTree().ChangeSceneToFile("res://MainScene/MainScene.tscn");
        }));

        SelectCountryPanel.Connect(SelectCountryPanel.SignalName.Back, Callable.From(() =>
        {
            mapScene.QueueFree();

            this.GetSelectEntity().Current = null;

            SeedPanel.Visible = true;
            SelectCountryPanel.Visible = false;
        }));
    }
}