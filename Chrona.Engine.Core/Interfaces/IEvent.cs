namespace Chrona.Engine.Core.Interfaces;

public interface IEvent
{
    string Title { get; }
    string Desc { get; }
    IOption Option { get; }
}