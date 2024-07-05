using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core.Events;

public class Event : IEvent
{
    public string Title => throw new NotImplementedException();

    public string Desc => throw new NotImplementedException();

    public IOption Option { get; }

    private IEventDef Def { get; }

    private ProcessContext context { get; }

    public Event(ProcessContext context, IEventDef def)
    {
        this.context = context;
        Def = def;

        Option = new Option(def.OptionDef, context);
    }

    internal void AIDo()
    {
        Option.Do();
    }
}

public struct ProcessContext : IProcessContext
{
    public IEntity From { get; }
    public IEntity To { get; }
    public ISession Session { get; }

    public ProcessContext(IEntity from, IEntity target, ISession session)
    {
        this.From = from;
        this.To = target;
        this.Session = session;
    }
}