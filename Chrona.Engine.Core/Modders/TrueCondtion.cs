using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Modders;

public class TrueCondtion : ICondition
{
    public bool IsSatisfied(IEntity entity, ISession session)
    {
        return true;
    }
}