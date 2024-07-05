namespace Chrona.Engine.Core.Interfaces;

public interface DataVisitor
{
    object Get(IEvent @event);
}
