using HuangD.Sessions.Maps.Builders;

namespace HuangD.Sessions.Maps;

public static class Builder
{
    public static Map Build(int maxSize, string seed)
    {
        var terrains = TerrainBuilder.Build(maxSize, seed);
        var pops = PopCountBuilder.Build(terrains, seed);

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

        return map;
    }

}