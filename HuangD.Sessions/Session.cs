using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Maps.Builders;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public class Session : AbstractSession
{
    public override IEntity Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override IEnumerable<IEntity> Entities => throw new System.NotImplementedException();

    public Dictionary<Index, MapCell> MapCells;

    public Dictionary<string, Province> Provinces;

    public Dictionary<string, Country> Countries;

    public Session()
    {
        MapCells = MapBuilder.Build2(64, "123");
        Provinces = Province.Builder.Build(MapCells.Values);
        Countries = Country.Builder.Build(Provinces.Values);
    }
}