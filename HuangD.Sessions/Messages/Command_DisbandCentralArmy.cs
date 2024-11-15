using Chrona.Engine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuangD.Sessions.Messages;

[RegistCommand]
public class Command_DisbandCentralArmy : IMessage
{
    public readonly string id;

    public Command_DisbandCentralArmy(string id)
    {
        this.id = id;
    }

    public object Target { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
