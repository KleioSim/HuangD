using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    public static class PopCountBuilder
    {
        private static readonly Dictionary<TerrainType, (int min, int max)> basePopDict = new Dictionary<TerrainType, (int min, int max)>()
        {
            { TerrainType.Land, (200000,500000)},
            { TerrainType.Hill, (50000,100000)},
            { TerrainType.Mount, (20000,50000)},
        };

        private static readonly Dictionary<TerrainType, (int min, int max)> incPopDict = new Dictionary<TerrainType, (int min, int max)>()
        {
            { TerrainType.Land, (20000,50000)},
            { TerrainType.Hill, (5000,10000)},
            { TerrainType.Mount, (2000,5000)},
        };

        public static Dictionary<Block, int> Build(Dictionary<Block, TerrainType> terrains, string seed)
        {
            var random = RandomBuilder.Build(seed);

            var rslt = terrains.Where(pair => pair.Value != TerrainType.Water)
                .ToDictionary(pair => pair.Key, pair =>
                {
                    var rangeBase = basePopDict[pair.Value];
                    var rangeIncs = pair.Key.Neighbors.Select(x => terrains[x])
                        .Where(x => x != TerrainType.Water)
                        .Select(x => incPopDict[x])
                        .ToArray();

                    return random.Next(rangeBase.min + rangeIncs.Sum(x => x.min), rangeBase.max + rangeIncs.Sum(x => x.max));
                });
            return rslt;
        }


        public static Dictionary<Index, int> Build(Dictionary<Index, TerrainType> terrainDict, string seed)
        {
            var random = RandomBuilder.Build(seed);
            var baseValueDict = terrainDict.Where(x => x.Value != TerrainType.Water).ToDictionary(k => k.Key, v =>
            {
                int popCount = 0;

                var index = v.Key;
                var terrain = v.Value;

                switch (terrain)
                {
                    case TerrainType.Land:
                        popCount = random.Next(1000, 5000);
                        break;
                    case TerrainType.Hill:
                        popCount = random.Next(100, 500);
                        break;
                    case TerrainType.Mount:
                        popCount = random.Next(10, 50);
                        break;
                }


                popCount += MapCell.IndexMethods.GetNeighborCells(index).Values.Select(neighbor =>
                {
                    if (terrainDict.TryGetValue(neighbor, out TerrainType neighborTerrain))
                    {
                        switch (neighborTerrain)
                        {
                            case TerrainType.Land:
                                return random.Next(500, 1000);
                            case TerrainType.Hill:
                                return random.Next(50, 100);
                            case TerrainType.Mount:
                                return 0;
                        }
                    }

                    return 0;
                }).Sum();

                return popCount;
            });

            return baseValueDict.ToDictionary(k => k.Key, v =>
            {
                var currIndex = v.Key;
                var currValue = v.Value;

                var neighorValues = MapCell.IndexMethods.GetNeighborCells(currIndex).Values
                    .Where(neighbor => baseValueDict.ContainsKey(neighbor))
                    .Select(neighbor => baseValueDict[neighbor]);

                return currValue * Math.Min(1, (int)(neighorValues.Average()) / 1000);
            });
        }
    }
}
