using Chrona.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_ChangeArmyLevel : IMessage
{
    public object Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public readonly string armyId;
    public readonly int level;

    public Command_ChangeArmyLevel(string armyId, int level)
    {
        this.armyId = armyId;
        this.level = level;
    }
}
