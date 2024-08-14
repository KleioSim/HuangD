using DynamicData;
using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace HuangD.Sessions;

public partial class Country
{
    private static Func<Country, IEnumerable<Province>> GetProvinces;

    public (float r, float g, float b) Color { get; set; }

    public string Key { get; }

    public IEnumerable<Province> Provinces => GetProvinces(this);

    public int PopCount => Provinces.Sum(x => x.PopCount);

    public IEnumerable<CentralArmy> CenterArmies => centralArmies;

    private List<CentralArmy> centralArmies;

    public Country(string key, (float, float, float) color)
    {
        Key = key;
        Color = color;

        centralArmies = new List<CentralArmy> { new CentralArmy(1000, 1000) };
    }
}
