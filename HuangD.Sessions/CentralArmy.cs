using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Utilties;
using System;

namespace HuangD.Sessions;

public class CentralArmy : IEntity
{
    public string Id { get; }
    public int Count { get; internal set; }
    public int ExpectCount { get; internal set; }
    public float Cost => Math.Max(ExpectCount, Count) / 1000;

    public Country Owner { get; internal set; }
    public Province Position { get; internal set; }
    public MoveTo MoveTo { get; internal set; }

    public CentralArmy(int count, int expectCount, Country owner)
    {
        Id = UUID.Generate("ARMY");

        Count = count;
        ExpectCount = expectCount;
        Owner = owner;
        Position = owner.CapitalProvince;
    }

    public void OnMove(Province province)
    {
        MoveTo = new MoveTo(province);
    }

    public void OnCancelMove()
    {
        MoveTo = null;
    }
}

public class MoveTo
{
    public Province Target { get; }

    public float speed { get; }

    public float percent { get; }

    public MoveTo(Province target)
    {
        Target = target;
    }

}