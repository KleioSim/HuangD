namespace Chrona.Engine.Core.Interfaces;

public interface ICondtionFactor
{
    public ICondition Condition { get; }

    public double Factor { get; }
}
