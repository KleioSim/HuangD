using Chrona.Engine.Core.Interfaces;

namespace HuangD.Sessions.Messages;

public class Command_Cancel_ArmyMove : IMessage
{
    public readonly string armyId;

    public Command_Cancel_ArmyMove(string armyId)
    {
        this.armyId = armyId;
    }

    public object Target { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public object Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

}