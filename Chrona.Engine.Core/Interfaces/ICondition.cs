namespace Chrona.Engine.Core.Interfaces;

public interface ICondition
{
    bool IsSatisfied(IEntity entity, ISession session);
}
