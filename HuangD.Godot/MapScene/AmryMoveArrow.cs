using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class AmryMoveArrow : ViewControl
{
    public static Func<Province, Province, (Vector2 position, float Rotation, float length)> CalcPositionAndRotation;

    public Button Cancel => GetNode<Button>("");
    public ProgressBar Progress => GetNode<ProgressBar>("");

    public string ArmyId
    {
        get => armyId;
        set
        {
            armyId = value;
            Update();
        }
    }

    private string armyId;

    protected override void Initialize()
    {
        Cancel.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnCancel));
    }

    protected override void Update()
    {
        var army = this.GetSession().Entities[armyId] as CentralArmy;
        if (army == null || army.MoveTo == null)
        {
            QueueFree();
            return;
        }

        var result = CalcPositionAndRotation(army.Position, army.MoveTo.Target);
        this.Position = result.position;
        this.Rotation = result.Rotation;
        this.Size = new Vector2(result.length, this.Size.Y);

        Progress.Value = army.MoveTo.percent;
    }

    private void OnCancel()
    {
        SendCommand(new Command_Cancel_ArmyMove(armyId));
    }
}
