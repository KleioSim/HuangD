using Chrona.Engine.Core.Interfaces;
using System;

namespace Chrona.Engine.Core.Modders;

public class OptionDef : IOptionDef
{
    public Func<IProcessContext, IEnumerable<IMessage>> ProductMessage { get; init; }

    public Func<IProcessContext, string> GetDesc { get; init; }
}

public abstract class EventDef : IEventDef
{
    public abstract IOptionDef OptionDef { get; }

    public abstract Func<IEntity, ISession, bool> IsSatisfied { get; }

    public abstract Func<IEntity, ISession, IEntity> FindTarget { get; }

    public abstract PlayerFlag playerFlag { get; }
}

public abstract class InteractionDef : IInteractionDef
{
    public abstract Func<IEntity, ISession, IEnumerable<IMessage>> Invoke { get; }

    public abstract string GetDesc(IEntity owner);

    public abstract IEnumerable<(bool flag, string desc)> GetVaildGroups(IEntity owner, ISession session);
}