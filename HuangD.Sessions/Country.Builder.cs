using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Country
{
    private static Func<Country, IEnumerable<Province>> GetProvinces;

    public string Key { get; }

    public IEnumerable<Province> Provinces => GetProvinces(this);

    public int PopCount => Provinces.Sum(x => x.PopCount);

    public IEnumerable<CentralArmy> CenterArmies => centralArmies;

    private List<CentralArmy> centralArmies;

    public Country()
    {
        Key = UUID.Generate("CNT");
        centralArmies = new List<CentralArmy> { new CentralArmy(1000, 1000) };
    }
}
