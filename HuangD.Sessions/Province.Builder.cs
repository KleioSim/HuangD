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

            var result = new Dictionary<Block, Province>();
            foreach (var pair in block2Terrain.Where(pair => pair.Value != TerrainType.Water))
            {
                var province = new Province(UUID.Generate("PROV"), GenerateProvinceName(result.Values.Select(x => x.Name)))
                {
                    BlockId = pair.Key.Id,
                    PopCount = pops[pair.Key]
                };

                result.Add(pair.Key, province);
            }

            return result;
        }

        private static string[] Names = { };

        private static string GenerateProvinceName(IEnumerable<string> usedNames)
        {
            return Names.Except(usedNames).First();
        }
    }
}
