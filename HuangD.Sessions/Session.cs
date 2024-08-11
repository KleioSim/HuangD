using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Maps.Builders;
using HuangD.Sessions.Messages;
using HuangD.Sessions.Utilties;
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

    public Country PlayerCountry;

    public Session(string seed)
    {
        UUID.Restart();

        MapCells = MapBuilder.Build2(64, seed);
        Provinces = Province.Builder.Build(MapCells.Values);
        Countries = Country.Builder.Build(Provinces.Values, Provinces.Values.Max(x => x.PopCount) * 3, Provinces.Count / 5, seed);
    }

    [MessageProcess]
    private void On_Command_ChangeProvinceOwner(Command_ChangeProvinceOwner cmd)
    {
        var province = Provinces[cmd.provinceId];
        var country = Countries[cmd.countryId];

        province.Owner = country;
    }
}