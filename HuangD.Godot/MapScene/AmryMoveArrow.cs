using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class AmryMoveArrow : ViewControl
{
    public static Func<Province, Province, (Vector2 position, float Rotation, float length)> CalcPositionAndRotation;

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

    }

    protected override void Update()
    {
        var army = this.GetSession().CentralArmies[armyId];
        if (army == null)
        {
            QueueFree();
        }

        this.Visible = army.MoveTo != null;

        var result = CalcPositionAndRotation(army.Position, army.MoveTo.Target);
        this.Position = result.position;
        this.Rotation = result.Rotation;
        this.Size = new Vector2(result.length, this.Size.Y);
    }
}
