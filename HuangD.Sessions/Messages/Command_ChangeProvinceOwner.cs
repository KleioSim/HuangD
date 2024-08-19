using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_ChangeProvinceOwner : IMessage
{
    public readonly string provinceId;
    public readonly string countryId;

    public Command_ChangeProvinceOwner(string provinceId, string countryId)
    {
        this.provinceId = provinceId;
        this.countryId = countryId;
    }

    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
