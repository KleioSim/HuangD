using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;
using HuangD.Sessions.Maps;
using HuangD.Sessions.Messages;
using HuangD.Sessions.Utilties;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Sessions.Maps.Builders.MapBuilder;

namespace HuangD.Sessions;

public class Session : AbstractSession
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

    public Dictionary<string, Block> Blocks { get; private set; }
    public Dictionary<string, TerrainType> Block2Terrain { get; private set; }
    public Dictionary<string, Province> Block2Province { get; private set; }
    public Dictionary<string, Province> Provinces { get; private set; }

    public Country PlayerCountry { get; private set; }

    public IEnumerable<string> CurrentReports => currentReports;

    public IEntity SelectedEntity { get; set; }

    private Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

    private List<string> currentReports = new List<string>();


    private static Session instance;

    static Session()
    {
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

        Date = new Date();

        var blocks = BlockBuilder.Build(120, 120, seed);
        var block2Terrain = TerrainBuilder.Build(blocks, seed);
        var block2province = Province.Builder.Build(block2Terrain, seed);

        Blocks = blocks.ToDictionary(b => b.Id, b => b);
        Block2Terrain = block2Terrain.ToDictionary(p => p.Key.Id, p => p.Value);
        Block2Province = block2province.ToDictionary(p => p.Key.Id, p => p.Value);
        Provinces = Block2Province.ToDictionary(p => p.Value.Id, p => p.Value);

        var countries = Country.Builder.Build(Provinces.Values, Provinces.Values.Max(x => x.PopCount) * 3, Provinces.Count() / 5, seed);
        var centralArmies = countries.Values.Select(x => new CentralArmy(1000, 1000, x)).ToDictionary(x => x.Id, y => y);

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