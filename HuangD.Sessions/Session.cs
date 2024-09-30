using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Maps.Builders;
using HuangD.Sessions.Messages;
using HuangD.Sessions.Utilties;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using static HuangD.Sessions.Maps.Builders.MapBuilder;

namespace HuangD.Sessions;

public class Session : AbstractSession
{
    public override IEntity Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override IReadOnlyDictionary<string, IEntity> Entities => entities;

    public Date Date { get; }

    //public Dictionary<Index, MapCell> MapCells { get; }

    public Country PlayerCountry { get; private set; }

    public IEnumerable<string> CurrentReports => currentReports;

    private Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

    private List<string> currentReports = new List<string>();

    public Session(string seed)
    {
        UUID.Restart();

        Province.FindArmies = (prov) => entities.Values.OfType<CentralArmy>().Where(x => x.Position == prov);
        Date = new Date();
        //MapCells = MapBuilder.Build2(64, seed);

        var blocks = BlockBuilder.Build(120, 120, seed);
        var terrains = TerrainBuilder.Build(blocks, seed);
        var pops = PopCountBuilder.Build(terrains, seed);

        var provinces = Province.Builder.Build(terrains, pops, seed);
        //var provinces = Province.Builder.Build(MapCells.Values, (prov) => entities.Values.OfType<CentralArmy>().Where(x => x.Position == prov));
        var countries = Country.Builder.Build(provinces.Values, provinces.Values.Max(x => x.PopCount) * 3, provinces.Count / 5, seed);
        var centralArmies = countries.Values.Select(x => new CentralArmy(1000, 1000, x)).ToDictionary(x => x.Id, y => y);

        foreach (var entity in provinces.Values)
        {
            entities.Add(entity.Id, entity);
        }

        foreach (var entity in countries.Values)
        {
            entities.Add(entity.Id, entity);
        }

        foreach (var entity in centralArmies.Values)
        {
            entities.Add(entity.Id, entity);
        }
    }

    [MessageProcess]
    private void On_Command_ChangeProvinceOwner(Command_ChangeProvinceOwner cmd)
    {
        var province = entities[cmd.provinceId] as Province;
        var country = entities[cmd.countryId] as Country;

        province.Owner = country;
    }

    [MessageProcess]
    private void On_Command_ChangePlayerCountry(Command_ChangePlayerCountry cmd)
    {
        PlayerCountry = entities[cmd.countryId] as Country;
    }

    [MessageProcess]
    private void On_Command_NextTurn(Command_NextTurn cmd)
    {
        currentReports.Clear();

        Date.DaysInc(10);


        foreach (var army in entities.Values.OfType<CentralArmy>())
        {
            army.OnNextTurn();
        }

        foreach (var battle in entities.Values.OfType<Province>().Select(x => x.Battle).Where(x => x != null))
        {
            currentReports.AddRange(battle.OnNextTurn(Date).Select(x => x.Desc));
        }
    }

    [MessageProcess]
    private void On_Command_ArmyMove(Command_ArmyMove cmd)
    {
        var army = entities[cmd.armyId] as CentralArmy;
        var province = entities[cmd.provinceId] as Province;

        army.OnMove(province);
    }

    [MessageProcess]
    private void On_Command_Cancel_ArmyMove(Command_Cancel_ArmyMove cmd)
    {
        var army = entities[cmd.armyId] as CentralArmy;
        if (army.IsRetreat)
        {
            throw new System.Exception();
        }

        army.OnCancelMove();
    }

    [MessageProcess]
    private void On_Commad_ArmyRetreat(Command_ArmyRetreat cmd)
    {
        var army = entities[cmd.armyId] as CentralArmy;
        if (army.MoveTo != null)
        {
            throw new System.Exception();
        }

        army.IsRetreat = true;

        var target = army.Position.Neighbors.FirstOrDefault(x => x.Owner == army.Owner);
        if (target == null)
        {
            target = army.Position.Neighbors.First();
        }

        army.OnMove(target);
    }
}