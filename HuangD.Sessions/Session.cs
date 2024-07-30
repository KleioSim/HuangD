using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;
using System.Collections.Generic;

namespace HuangD.Sessions;

public class Session : AbstractSession
{
    public override IEntity Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override IEnumerable<IEntity> Entities => throw new System.NotImplementedException();

    public Dictionary<Index, MapCell> MapCells = Maps.MapBuilder.Build2(64, "123");
}