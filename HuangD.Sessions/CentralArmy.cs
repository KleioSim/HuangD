using HuangD.Sessions.Utilties;
using System;

namespace HuangD.Sessions;

public class CentralArmy : Army
{
    public override string Id { get; }
    public override float Cost => Math.Max(ExpectCount, Count) / 1000;
    public override Country Owner { get; }

    public MoveTo MoveTo { get; internal set; }

    internal CentralArmy(int count, int expectCount, Country owner)
    {
        Id = UUID.Generate("ARMY");

        Count = count;
        ExpectCount = expectCount;
        Owner = owner;
        Position = owner.CapitalProvince;
    }

    internal void OnMove(Province province)
    {
        if (MoveTo != null && MoveTo.Target == province)
        {
            return;
        }

        MoveTo = null;
        if (Position == province)
        {
            return;
        }

        MoveTo = new MoveTo(province);
        Position.UpdateBattle();
    }

    internal void OnCancelMove()
    {
        MoveTo = null;
    }

    internal void OnNextTurn()
    {
        if (MoveTo != null)
        {
            MoveTo.percent += MoveTo.speed;
            if (MoveTo.percent >= 100)
            {
                Position = MoveTo.Target;
                MoveTo = null;

                Position.UpdateBattle();
            }
        }
    }
}

public class MoveTo
{
    public Province Target { get; }

    public float speed { get; } = 30;

    public float percent { get; internal set; }

    internal MoveTo(Province target)
    {
        Target = target;
    }
}
