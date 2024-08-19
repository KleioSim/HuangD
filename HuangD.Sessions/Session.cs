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


    public Date Date { get; }

    public Dictionary<Index, MapCell> MapCells { get; }

    public Dictionary<string, Province> Provinces { get; }

    public Dictionary<string, Country> Countries { get; }

    public Country PlayerCountry { get; private set; }

    public Dictionary<string, CentralArmy> CentralArmies { get; }

    public Session(string seed)
    {
        UUID.Restart();

        Date = new Date();
        MapCells = MapBuilder.Build2(64, seed);
        Provinces = Province.Builder.Build(MapCells.Values, (prov) => CentralArmies.Values.Where(x => x.Position == prov));
        Countries = Country.Builder.Build(Provinces.Values, Provinces.Values.Max(x => x.PopCount) * 3, Provinces.Count / 5, seed);

        CentralArmies = Countries.Values.Select(x => new CentralArmy(1000, 1000, x)).ToDictionary(x => x.Key, y => y);
    }

    [MessageProcess]
    private void On_Command_ChangeProvinceOwner(Command_ChangeProvinceOwner cmd)
    {
        var province = Provinces[cmd.provinceId];
        var country = Countries[cmd.countryId];

        province.Owner = country;
    }

    [MessageProcess]
    private void On_Command_ChangePlayerCountry(Command_ChangePlayerCountry cmd)
    {
        PlayerCountry = Countries[cmd.countryId];
    }

    [MessageProcess]
    private void On_Command_NextTurn(Command_NextTurn cmd)
    {
        Date.DaysInc(10);
    }

    [MessageProcess]
    private void On_Command_ArmyMove(Command_ArmyMove cmd)
    {
        var army = CentralArmies[cmd.armyId];
        var province = Provinces[cmd.provinceId];

        army.OnMove(province);
    }
}