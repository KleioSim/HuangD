using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class AmryMoveArrow : Control, IView<ISessionData>
{
    public static Func<string, PoliticalItem> GetPoliticalItem { get; set; }

    public Button Cancel => GetNode<Button>("HBoxContainer/Button");
    public TextureProgressBar Progress => GetNode<TextureProgressBar>("HBoxContainer/TextureProgressBar");
    public MapScene MapScene { get; internal set; }

    public string ArmyId
    {
        get => armyId;
        set
        {
            armyId = value;

            var view = this as IView<ISessionData>;
            view.IsSelfDirty = true;
        }
    }

    private string armyId;

    public void OnZoom(Vector2 vector)
    {
        var view = this as IView<ISessionData>;
        view.IsSelfDirty = true;
    }


    public override void _Ready()
    {
        Cancel.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            this.GetSession().OnMessage(new Command_Cancel_ArmyMove(ArmyId));
        }));
    }

    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        var army = this.GetSession().Entities[armyId] as CentralArmy;
        if (army == null || army.MoveTo == null)
        {
            QueueFree();
            return;
        }

        //Cancel.Visible = !army.IsRetreat;

        var fromPolitical = GetPoliticalItem(army.Position.Id);
        var targetPolitical = GetPoliticalItem(army.MoveTo.Target.Id);

        var fromPos = fromPolitical.ArmyInfo.ArmyIcon.GetGlobalPositionWithPivotOffset();
        var targetPos = targetPolitical.MoveTarget.GetGlobalPositionWithPivotOffset();

        var position = targetPos;
        var angle = (float)(Math.Atan2((targetPos.Y - fromPos.Y), (targetPos.X - fromPos.X)) * 180 / Math.PI) + 90;
        var length = fromPos.DistanceTo(targetPos);

        GD.Print($"targetPolitical.MoveTarget position:{targetPolitical.MoveTarget.GetGlobalPositionWithPivotOffset()}");

        this.SetGlobalPositionWithPivotOffset(position);
        this.RotationDegrees = angle;
        this.Size = new Vector2(this.Size.X, length);

        Progress.Value = army.MoveTo.percent;
    }
}
