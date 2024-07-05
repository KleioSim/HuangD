namespace Chrona.Engine.Core.Interfaces;

public interface IInteractionDef
{
    string GetDesc(IEntity owner);

    IEnumerable<(bool flag, string desc)> GetVaildGroups(IEntity owner, ISession session);

    Func<IEntity, ISession, IEnumerable<IMessage>> Invoke { get; }
}
