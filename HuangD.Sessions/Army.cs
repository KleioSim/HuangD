using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions;

public abstract class Army : IEntity
{
    public abstract string Id { get; }
    public abstract float Cost { get; }
    public abstract Country Owner { get; }

    public int Count { get; internal set; }
    public int ExpectCount { get; internal set; }
    public Province Position { get; internal set; }
}