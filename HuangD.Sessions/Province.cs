using HuangD.Sessions.Maps;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Province
{
    public string Key { get; }

    public Country Owner { get; set; }

    public IEnumerable<Province> Neighbors { get; private set; } = new Province[] { };

    public IEnumerable<MapCell> MapCells { get; private set; }

    public int PopCount => MapCells.Sum(x => x.PopCount);

    public Province(string key, IEnumerable<MapCell> mapCells)
    {
        Key = key;
        MapCells = mapCells;
    }

}