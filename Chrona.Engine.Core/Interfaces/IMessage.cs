namespace Chrona.Engine.Core.Interfaces;

public interface IMessage
{
    public object Target { get; set; }
    public object Value { get; set; }
}
