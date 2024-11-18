using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Messages;
using HuangD.Sessions.Utilties;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using static HuangD.Sessions.Maps.Builders.MapBuilder;

namespace HuangD.Sessions;

public interface ISessionData : ISession
{
    Date Date { get; }
    Country PlayerCountry { get; }
    //IEntity SelectedEntity { get; set; }

    (int x, int y) MapSize { get; }
    Dictionary<string, Block> Blocks { get; }
    Dictionary<string, TerrainType> Block2Terrain { get; }
    Dictionary<string, Province> Block2Province { get; }
    ProvinceDictionary Provinces { get; }
}

public class ProvinceDictionary : IReadOnlyDictionary<string, Province>
{
    private Dictionary<string, Province> Id2Province;
    private Dictionary<Index, Province> Index2Province;

    public Province this[string key] => ((IReadOnlyDictionary<string, Province>)Id2Province)[key];

    public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, Province>)Id2Province).Keys;

    public IEnumerable<Province> Values => ((IReadOnlyDictionary<string, Province>)Id2Province).Values;

    public int Count => ((IReadOnlyCollection<KeyValuePair<string, Province>>)Id2Province).Count;

    public ProvinceDictionary(Dictionary<string, Province> id2Province)
    {
        Id2Province = id2Province;

        Index2Province = new Dictionary<Index, Province>();
        foreach (var province in id2Province.Values)
        {
            foreach (var index in province.Block.Indexes)
            {
                Index2Province.Add(index, province);
            }
        }
    }

    public bool ContainsKey(string key)
    {
        return ((IReadOnlyDictionary<string, Province>)Id2Province).ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, Province>> GetEnumerator()
    {
        return ((IEnumerable<KeyValuePair<string, Province>>)Id2Province).GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Province value)
    {
        return ((IReadOnlyDictionary<string, Province>)Id2Province).TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Id2Province).GetEnumerator();
    }

    public Province GetByIndex(Index index)
    {
        if (Index2Province.TryGetValue(index, out var ret))
        {
            return ret;
        }

        return null;
    }
}

public class War : IEntity
{
    public War(Country from, Country target)
    {
        this.from = from;
        this.target = target;

        Id = UUID.Generate("WAR");
    }

    public Country from { get; set; }
    public Country target { get; set; }

    public string Id { get; }
}

public class Session : AbstractSession, ISessionData
{
    public static Session Instance
    {
        get
        {
            instance ??= new Session();
            return instance;
        }
    }

    public override IEntity Player { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override IReadOnlyDictionary<string, IEntity> Entities => entities;

    public Date Date { get; private set; }

    public (int x, int y) MapSize { get; private set; }
    public Dictionary<string, Block> Blocks { get; private set; }
    public Dictionary<string, TerrainType> Block2Terrain { get; private set; }
    public Dictionary<string, Province> Block2Province { get; private set; }
    public ProvinceDictionary Provinces { get; private set; }
    public IEnumerable<War> Wars => entities.Values.OfType<War>();

    public Country PlayerCountry { get; private set; }

    public IEnumerable<string> CurrentReports => currentReports;



    //public IEntity SelectedEntity { get; set; }

    private Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

    private List<string> currentReports = new List<string>();

    private static Session instance;

    static Session()
    {
        Country.GetCenterArmies = (country) => instance.entities.Values.OfType<CentralArmy>().Where(x => x.Owner == country);
        Country.GetProvinces = (coutry) => instance.Provinces.Values.Where(x => x.Owner == coutry);
        Country.GetWars = (country) => instance.Wars.Where(x => x.from == country || x.target == country);

        Province.GetBlock = (blockId) => instance.Blocks[blockId];
        Province.GetTerrain = (blockId) => instance.Block2Terrain[blockId];
        Province.GetNeighbors = (blockId) => instance.Blocks[blockId].Neighbors
            .Where(x => instance.Block2Province.ContainsKey(x.Id))
            .Select(x => instance.Block2Province[x.Id]);
        Province.FindArmies = (prov) => instance.entities.Values.OfType<CentralArmy>().Where(x => x.Position == prov);
    }

    private Session()
    {

    }

    public void Init(string seed)
    {
        UUID.Restart();
        entities.Clear();

        Date = new Date();

        MapSize = (120, 120);
        var blocks = BlockBuilder.Build(MapSize.x, MapSize.y, seed);
        var block2Terrain = TerrainBuilder.Build(blocks, seed);
        var block2province = Province.Builder.Build(block2Terrain, seed);

        Blocks = blocks.ToDictionary(b => b.Id, b => b);
        Block2Terrain = block2Terrain.ToDictionary(p => p.Key.Id, p => p.Value);
        Block2Province = block2province.ToDictionary(p => p.Key.Id, p => p.Value);
        Provinces = new ProvinceDictionary(Block2Province.ToDictionary(p => p.Value.Id, p => p.Value));

        var countries = Country.Builder.Build(Provinces.Values, (int)Provinces.Values.Average(x => x.PopCount) * 4, 4, seed);
        var centralArmies = countries.Values.Select(x => new CentralArmy(x, ArmyLevel.VHIGH)).ToDictionary(x => x.Id, y => y);

        foreach (var entity in Provinces.Values)
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

        foreach (var entity in Provinces.Values.Select(x => x.LocalArmy))
        {
            entities.Add(entity.Id, entity);
        }

        //entities.Add(nameof(PlayerArmy), new PlayerArmy(this));
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
        foreach (var army in entities.Values.OfType<LocalArmy>())
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

    //[MessageProcess]
    //private void On_Command_SelectEntity(Command_SelectEntity cmd)
    //{
    //    SelectedEntity = cmd.entityId == null ? null : entities[cmd.entityId];
    //}

    [MessageProcess]
    private void On_Command_ChangeArmyLevel(Command_ChangeArmyLevel cmd)
    {
        var army = entities[cmd.armyId] as Army;
        army.Level = (ArmyLevel)cmd.level;
    }

    [MessageProcess]
    private void On_Command_CreateCenterlArmy(Command_CreateCenterlArmy cmd)
    {
        var country = entities[cmd.countryId] as Country;
        var army = new CentralArmy(country, (ArmyLevel)cmd.level);
        entities.Add(army.Id, army);
    }

    [MessageProcess]
    private void On_Command_DisbandCentralArmy(Command_DisbandCentralArmy cmd)
    {
        var army = entities[cmd.id] as Army;
        entities.Remove(army.Id);
    }

    [MessageProcess]
    private void On_Command_WarStart(Command_WarStart cmd)
    {
        var war = new War(entities[cmd.fromId] as Country, entities[cmd.targetId] as Country);
        entities.Add(war.Id, war);
    }

    [MessageProcess]
    private void On_Command_WarEnd(Command_WarEnd cmd)
    {
        var war = entities[cmd.Id] as War;
        entities.Remove(war.Id);
    }
}