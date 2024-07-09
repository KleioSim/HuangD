using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;

namespace HuangD.Sessions;

public class Session : AbstractSession
{
    public override IEntity Player { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override IEnumerable<IEntity> Entities => throw new NotImplementedException();

    public Map Map = Maps.Builder.Build(64, "123");
}