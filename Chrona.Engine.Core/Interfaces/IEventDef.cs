namespace Chrona.Engine.Core.Interfaces;

public enum PlayerFlag
{
    ForPlayerAndId,
    ForAI,
}

public interface IEventDef
{

    Func<IEntity, ISession, bool> IsSatisfied { get; }
    Func<IEntity, ISession, IEntity> FindTarget { get; }

    IOptionDef OptionDef { get; }
    PlayerFlag playerFlag { get; }
}