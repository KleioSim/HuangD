﻿using Chrona.Engine.Core.Interfaces;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Province : IEntity
{
    public string Id { get; }

    public Country Owner { get; set; }

    public PopTax PopTax { get; }

    public IEnumerable<CentralArmy> centralArmies => FindArmies();

    public IEnumerable<Province> Neighbors { get; private set; } = new Province[] { };

    public IEnumerable<MapCell> MapCells { get; private set; }

    public int PopCount => MapCells.Sum(x => x.PopCount);

    private Func<IEnumerable<CentralArmy>> FindArmies { get; set; }
    public bool IsInteractionDateOut { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public IEnumerable<IInteraction> Interactions => throw new NotImplementedException();

    public Province(string key, IEnumerable<MapCell> mapCells, Func<Province, IEnumerable<CentralArmy>> armyFinder)
    {
        Id = key;
        MapCells = mapCells;
        FindArmies = () => armyFinder(this);
        PopTax = new PopTax(this);
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