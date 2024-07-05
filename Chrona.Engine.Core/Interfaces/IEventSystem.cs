namespace Chrona.Engine.Core.Interfaces;

internal interface IEventSystem
{
    IEnumerable<IEvent> OnNexturn(ISession session, Dictionary<Type, IEnumerable<IEventDef>> eventDefs);
}
