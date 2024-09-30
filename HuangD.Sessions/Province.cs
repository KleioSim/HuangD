using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Province : IEntity
{
    public string Id { get; }

    public Country Owner
    {
        get => owner;
        set
        {
            if (owner == value)
            {
                return;
            }

            owner = value;

            UpdateBattle();
        }
    }

    public TerrainType Terrain { get; init; }
    public IEnumerable<Maps.Index> Indexes { get; init; }
    public Maps.Index CoreIndex { get; init; }

    public PopTax PopTax { get; }

    public LocalArmy LocalArmy { get; }

    public IEnumerable<CentralArmy> centralArmies => FindArmies(this);

    public IEnumerable<Province> Neighbors { get; private set; } = new Province[] { };

    public IEnumerable<MapCell> MapCells { get; private set; }

    public int PopCount { get; set; }

    public Battle Battle { get; private set; }

    public static Func<Province, IEnumerable<CentralArmy>> FindArmies { get; set; }

    private Country owner;

    private Province(string id)
    {
        Id = id;

        PopTax = new PopTax(this);
        LocalArmy = new LocalArmy(this);
    }

    public Province(string id, IEnumerable<Maps.Index> indexes, Maps.Index coreIndex, TerrainType terrain, int popCount)
    {
        Id = id;
        Indexes = indexes;
        CoreIndex = coreIndex;
        PopCount = popCount;
        Terrain = terrain;

        PopTax = new PopTax(this);
        LocalArmy = new LocalArmy(this);
    }

    public Province(string key, IEnumerable<MapCell> mapCells, Func<Province, IEnumerable<CentralArmy>> armyFinder)
    {
        Id = key;
        MapCells = mapCells;
        PopTax = new PopTax(this);
        LocalArmy = new LocalArmy(this);
    }

    internal void UpdateBattle()
    {
        var enemies = centralArmies.Where(x => x.Owner != Owner && x.MoveTo == null).ToArray();
        if (enemies.Length == 0)
        {
            Battle = null;
            return;
        }

        Battle ??= new Battle(this);
    }
}

public class PopTax
{
    public float Current => Province.PopCount / 10000;

    public Province Province { get; }

    public PopTax(Province province)
    {
        Province = province;
    }

}
