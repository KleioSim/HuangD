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

namespace HuangD.Sessions;

public class Session : AbstractSession
{
    public override IEntity Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override IReadOnlyDictionary<string, IEntity> Entities => entityDict;


    public Date Date { get; }

    public Dictionary<Index, MapCell> MapCells { get; }

    public Dictionary<string, Province> Provinces { get; }

    public Dictionary<string, Country> Countries { get; }

    public Country PlayerCountry { get; private set; }

    public Dictionary<string, CentralArmy> CentralArmies { get; }

    private EntityDictionary entityDict = new EntityDictionary();


    public Session(string seed)
    {
        UUID.Restart();

        Date = new Date();
        MapCells = MapBuilder.Build2(64, seed);
        Provinces = Province.Builder.Build(MapCells.Values, (prov) => CentralArmies.Values.Where(x => x.Position == prov));
        Countries = Country.Builder.Build(Provinces.Values, Provinces.Values.Max(x => x.PopCount) * 3, Provinces.Count / 5, seed);

        CentralArmies = Countries.Values.Select(x => new CentralArmy(1000, 1000, x)).ToDictionary(x => x.Id, y => y);

        foreach (var entity in Provinces.Values)
        {
            entityDict.Add(entity);
        }

        foreach (var entity in Countries.Values)
        {
            entityDict.Add(entity);
        }

        foreach (var entity in CentralArmies.Values)
        {
            entityDict.Add(entity);
        }
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

        foreach (var army in CentralArmies.Values)
        {
            army.OnNextTurn();
        }
    }

    [MessageProcess]
    private void On_Command_ArmyMove(Command_ArmyMove cmd)
    {
        var army = CentralArmies[cmd.armyId];
        var province = Provinces[cmd.provinceId];

        army.OnMove(province);
    }

    [MessageProcess]
    private void On_Command_Cancel_ArmyMove(Command_Cancel_ArmyMove cmd)
    {
        var army = CentralArmies[cmd.armyId];

        army.OnCancelMove();
    }
}

public class EntityDictionary : IReadOnlyDictionary<string, IEntity>
{
    public IEntity this[string key] => throw new System.NotImplementedException();

    public IEnumerable<string> Keys => entities.Select(x => x.Key);

    public IEnumerable<IEntity> Values => entities.Select(x => x.Value);

    public int Count => entities.Count();

    private ConditionalWeakTable<string, IEntity> entities = new ConditionalWeakTable<string, IEntity>();


    public bool ContainsKey(string key)
    {
        return entities.TryGetValue(key, out IEntity entity);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out IEntity enitity)
    {
        enitity = null;
        return entities.TryGetValue(key, out enitity);
    }

    public IEnumerator<KeyValuePair<string, IEntity>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, IEntity>>)entities).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, IEntity>>)entities).GetEnumerator();
    }

    internal void Add(IEntity entity)
    {
        entities.Add(entity.Id, entity);
    }
}