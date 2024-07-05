using Chrona.Engine.Core.Events;

namespace Chrona.Engine.Core.Interfaces;

public interface IOption
{
    string Desc { get; }
    string Tip { get; }

    void Do();
}

public interface IOptionDef
{
    Func<IProcessContext, string> GetDesc { get; }
    Func<IProcessContext, IEnumerable<IMessage>> ProductMessage { get; }
}

public interface IProcessContext
{
    IEntity From { get; }
    IEntity To { get; }
    ISession Session { get; }
}