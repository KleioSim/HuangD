using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

public class Command_SelectEntity : IMessage
{
    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public readonly string entityId;

    public Command_SelectEntity(string entityId)
    {
        this.entityId = entityId;
    }
}