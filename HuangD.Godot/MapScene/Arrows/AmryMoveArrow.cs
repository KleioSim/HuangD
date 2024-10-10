using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class AmryMoveArrow : ViewControl
{
    public Button Cancel => GetNode<Button>("HBoxContainer/Button");
    public TextureProgressBar Progress => GetNode<TextureProgressBar>("HBoxContainer/TextureProgressBar");
    public MapScene MapScene { get; internal set; }

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

    public void OnZoom()
    {
        Update();
    }

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

        Cancel.Visible = !army.IsRetreat;

        var result = MapScene.CalcPositionAndRotation(army.Position, army.MoveTo.Target);
        this.SetGlobalPositionWithPivotOffset(result.position);
        this.RotationDegrees = result.Rotation;
        this.Size = new Vector2(this.Size.X, result.length);

        Progress.Value = army.MoveTo.percent;
    }

    private void OnCancel()
    {
        SendCommand(new Command_Cancel_ArmyMove(armyId));

        MapScene.UpdateMoveInfo(armyId);
    }
}
