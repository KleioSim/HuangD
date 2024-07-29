using HuangD.Sessions.Maps.Builders;

namespace HuangD.Sessions.Maps;

public static class Builder
{
    public static Map Build(int maxSize, string seed)
    {
        var terrains = TerrainBuilder.Build(maxSize, seed);
        var pops = PopCountBuilder.Build(terrains, seed);
        var province = ProvinceBuilder.Build(pops, seed, pops.Values.Max() * 9, pops.Count() / 20);

        var map = new Map();
        map.Terrains.Edit(innerCache =>
        {
            foreach (var pair in terrains)
            {
                innerCache.AddOrUpdate(new TerrainItem() { Index = pair.Key, Type = pair.Value });
            }
        });

        map.Pops.Edit(innerCache =>
        {
            foreach (var pair in pops)
            {
                innerCache.AddOrUpdate(new PopItem() { Index = pair.Key, Count = pair.Value });
            }
        });

        map.Provinces.Edit(innerCache =>
        {
            foreach (var pair in province)
            {
                innerCache.AddOrUpdate(new ProvinceCell() { Index = pair.Key, ProvinceId = pair.Value });
            }
        });

        return map;
    }

}