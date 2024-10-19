using HuangD.Sessions.Utilties;
using System;

namespace HuangD.Sessions;

public class LocalArmy : Army
{
    public override string Id { get; }

    public override float Cost => Math.Max(ExpectCount, Count) / 5000;

    public override int ExpectCount => ((int)Level + 1) * 500;
    public override Country Owner => Province.Owner;
    public Province Province { get; }

    public LocalArmy(Province province)
    {
        Id = UUID.Generate("ARMY");
        this.Province = province;
        Level = ArmyLevel.Low;

        Count = ExpectCount;
    }

    internal void OnNextTurn()
    {
        Count = ExpectCount;
    }
}