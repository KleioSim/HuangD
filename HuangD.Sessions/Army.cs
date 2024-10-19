using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions;

public abstract class Army : IEntity
{
    public abstract string Id { get; }
    public abstract float Cost { get; }
    public abstract Country Owner { get; }

    public ArmyLevel Level { get; internal set; }
    public int Count { get; internal set; }
    public virtual int ExpectCount { get; internal set; }
    public Province Position { get; internal set; }
}

public enum ArmyLevel
{
    VeryLow,
    Low,
    Mid,
    High,
    VeryHigh,
}