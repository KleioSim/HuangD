using Chrona.Engine.Core.Sessions;

namespace Chrona.Engine.Core.Interfaces;

public interface ITargetFinder
{
    IEnumerable<ICondtionFactor> ConditionFactors { get; }

    IEventTarget Targets { get; }
}
