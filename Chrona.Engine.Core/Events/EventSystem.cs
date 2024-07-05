
using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

internal class EventSystem : IEventSystem
{
    private Random random = new Random();

    public IEnumerable<IEvent> OnNexturn(ISession session, Dictionary<Type, IEnumerable<IEventDef>> eventDefs)
    {
        foreach (var from in session.Entities)
        {
            foreach (var eventDef in eventDefs[from.GetType()])
            {
                if (eventDef.playerFlag == PlayerFlag.ForAI && from == session.Player)
                {
                    continue;
                }

                if (!eventDef.IsSatisfied(from, session))
                {
                    continue;
                }

                var target = eventDef.FindTarget(from, session);
                if (target == null)
                {
                    continue;
                }

                var context = new ProcessContext(from, target, session);
                var @event = new Event(context, eventDef);
                if (target != session.Player)
                {
                    @event.AIDo();
                    continue;
                }

                yield return @event;
            }
        }
    }
}