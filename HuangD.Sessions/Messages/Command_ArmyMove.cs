using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

public class Command_ArmyMove : IMessage
{
    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public readonly string armyId;
    public readonly string provinceId;

    public Command_ArmyMove(string armyId, string provinceId)
    {
        this.armyId = armyId;
        this.provinceId = provinceId;
    }
}