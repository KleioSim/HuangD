using Chrona.Engine.Core.Interfaces;
using System;

namespace HuangD.Sessions;

public abstract class Army : IEntity
{
    public abstract string Id { get; }
    public abstract float Cost { get; }
    public abstract Country Owner { get; }

    public ArmyLevel Level
    {
        get => armyLevel;
        internal set
        {
            if (armyLevel == value) return;
            armyLevel = value;

            Count = Math.Min(ExpectCount, Count);
        }
    }

    public int Count { get; internal set; }
    public virtual int ExpectCount { get; internal set; }
    public virtual int IncCount { get; internal set; } = 100;
    public Province Position { get; internal set; }

    public ArmyLevel armyLevel;

    internal virtual void OnNextTurn()
    {
        Count += IncCount;
        Count = Math.Min(ExpectCount, Count);
    }
}

public enum ArmyLevel
{
    VeryLow,
    Low,
    Mid,
    High,
    VeryHigh,
}