using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_CreateCenterlArmy : IMessage
{
    internal readonly string countryId;
    internal readonly int level;

    public Command_CreateCenterlArmy(string countryId, int level)
    {
        this.countryId = countryId;
        this.level = level;
    }

    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
