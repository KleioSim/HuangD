namespace Chrona.Engine.Core.Interfaces;

public interface IInteraction
{
    string Desc { get; }

    void Invoke(ISession session);

    IEnumerable<(bool flag, string desc)> GetVaildGroups(ISession session);
}