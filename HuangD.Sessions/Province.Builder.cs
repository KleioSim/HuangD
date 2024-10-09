using HuangD.Sessions.Maps;
using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using static HuangD.Sessions.Maps.Builders.MapBuilder;

namespace HuangD.Sessions;

public partial class Province
{
    public static class Builder
    {
        public static Dictionary<Block, Province> Build(Dictionary<Block, TerrainType> block2Terrain, string seed)
        {
            var pops = PopCountBuilder.Build(block2Terrain, seed);

            var provinces = block2Terrain.Where(pair => pair.Value != TerrainType.Water)
                .ToDictionary(pair => pair.Key, pair =>
                {
                    var province = new Province(UUID.Generate("PROV"))
                    {
                        BlockId = pair.Key.Id,
                        PopCount = pops[pair.Key]
                    };
                    return province;
                });

            return provinces;
        }
    }
}
