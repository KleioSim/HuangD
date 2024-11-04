using Chrona.Engine.Core.Interfaces;
using DynamicData;
using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace HuangD.Sessions;

public partial class Country : IEntity
{
    internal static Func<Country, IEnumerable<Province>> GetProvinces;
    internal static Func<Country, IEnumerable<CentralArmy>> GetCenterArmies;

    public (float h, float s, float v) Color { get; }

    public string Id { get; }

    public IEnumerable<Province> Provinces => GetProvinces(this);
    public IEnumerable<CentralArmy> CenterArmies => GetCenterArmies(this);

    public Economy Economy { get; }

    public Province CapitalProvince
    {
        get => capitalProvince;
        set
        {
            if (!Provinces.Contains(value))
            {
                throw new Exception();
            }

            capitalProvince = value;
        }
    }

    public int PopCount => Provinces.Sum(x => x.PopCount);

    private Province capitalProvince;

    public Country(string key, (float h, float s, float v) color)
    {
        Id = key;
        Color = color;
        Economy = new Economy(this);
    }
}
