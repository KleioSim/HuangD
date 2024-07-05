namespace Chrona.Engine.Core.Interfaces;

public interface ISession
{
    int UpdateFlag { get; }
    IEntity Player { get; }
    IEnumerable<IEntity> Entities { get; }
    void OnNextTurn();
    void OnMessage(IMessage message);

    IModder Modder { get; set; }
}
