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
    internal static Func<Country, IEnumerable<War>> GetWars;

    public (float h, float s, float v) Color { get; }

    public string Name { get; internal set; }
    public string Id { get; }

    public IEnumerable<Province> Provinces => GetProvinces(this);
    public IEnumerable<CentralArmy> CenterArmies => GetCenterArmies(this);
    public IEnumerable<War> Wars => GetWars(this);

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

    public Country((float h, float s, float v) color)
    {
        Id = UUID.Generate("CNT");
        Color = color;
        Economy = new Economy(this);
    }
}
