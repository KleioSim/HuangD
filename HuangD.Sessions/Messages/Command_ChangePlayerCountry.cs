using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

public class Command_ChangePlayerCountry : IMessage
{
    public readonly string countryId;

    public Command_ChangePlayerCountry(string countryId)
    {
        this.countryId = countryId;
    }

    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}