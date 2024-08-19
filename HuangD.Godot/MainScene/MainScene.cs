using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Messages;
using System;

public partial class MainScene : ViewControl
{
    Button NextTurn => GetNode<Button>("");

    protected override void Initialize()
    {
        NextTurn.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnNextTurn));
    }

    protected override void Update()
    {

    }

    private void OnNextTurn()
    {
        SendCommand(new Command_NextTurn());
    }
}
