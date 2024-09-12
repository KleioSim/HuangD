using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

internal class Command_ArmyRetreat : IMessage
{
    public readonly string armyId;

    public Command_ArmyRetreat(string armyId)
    {
        this.armyId = armyId;
    }

    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}
