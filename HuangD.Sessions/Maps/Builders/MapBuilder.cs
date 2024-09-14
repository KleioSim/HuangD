using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static Dictionary<Index, MapCell> Build2(int maxSize, string seed)
    {
        var terrains = TerrainBuilder.Build(maxSize, seed);
        var pops = PopCountBuilder.Build(terrains, seed);
        var provinces = ProvinceBuilder.Build(pops, seed, pops.Values.Max() * 9, pops.Count() / 20);

        var dict = new Dictionary<Index, MapCell>();
        foreach (var key in terrains.Keys)
        {
            terrains.TryGetValue(key, out var terrainType);
            pops.TryGetValue(key, out var popCount);
            provinces.TryGetValue(key, out var provinceId);

            var mapCell = new MapCell(key, terrainType, popCount, provinceId);
            dict.Add(key, mapCell);
        }
        return dict;
    }

}