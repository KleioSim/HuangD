using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Province : IEntity
{
    public static Func<string, TerrainType> GetTerrain { get; set; }
    public static Func<string, Block> GetBlock { get; set; }
    public static Func<string, IEnumerable<Province>> GetNeighbors { get; set; }

    public string Id { get; }
    public string BlockId { get; init; }

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

    public TerrainType Terrain => GetTerrain(BlockId);
    public Block Block => GetBlock(BlockId);
    public IEnumerable<Province> Neighbors => GetNeighbors(BlockId);

    public PopTax PopTax { get; }

    public LocalArmy LocalArmy { get; }

    public IEnumerable<CentralArmy> centralArmies => FindArmies(this);

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
