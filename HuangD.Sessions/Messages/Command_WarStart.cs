using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_WarStart : IMessage
{
    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public readonly string fromId;
    public readonly string targetId;

    public Command_WarStart(string fromId, string targetId)
    {
        this.fromId = fromId;
        this.targetId = targetId;
    }
}