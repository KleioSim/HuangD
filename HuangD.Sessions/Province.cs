using HuangD.Sessions.Maps;
using System.Collections.Generic;

namespace HuangD.Sessions;

public partial class Province
{
    public string Key { get; }

    public IEnumerable<Province> Neighbors { get; private set; } = new Province[] { };

    public IEnumerable<MapCell> MapCells { get; private set; }

    public Province(string key, IEnumerable<MapCell> mapCells)
    {
        Key = key;
        MapCells = mapCells;
    }

}