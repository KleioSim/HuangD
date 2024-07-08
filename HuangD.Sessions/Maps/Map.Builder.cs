namespace HuangD.Sessions.Maps;

public partial class Map
{
    public static class Builder
    {
        public static Map Build(int maxSize, string seed)
        {
            var dict = TerrainBuilder.Build(maxSize, seed);

            var map = new Map();
            map.Terrains.Edit(innerCache =>
            {
                foreach (var pair in dict)
                {
                    innerCache.AddOrUpdate(new TerrainItem() { Index = pair.Key, Type = pair.Value });
                }
            });
            return map;
        }

    }
}