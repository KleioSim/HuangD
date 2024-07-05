namespace Chrona.Engine.Core.Interfaces;

public interface IEventTarget
{
    IEnumerable<IEntity> Get(IEntity entity, ISession session);
}
