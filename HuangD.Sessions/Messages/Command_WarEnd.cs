using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_WarEnd : IMessage
{
    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public readonly string Id;

    public Command_WarEnd(string Id)
    {
        this.Id = Id;
    }
}