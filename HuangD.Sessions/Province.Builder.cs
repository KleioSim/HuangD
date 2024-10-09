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
                        Indexes = pair.Key.Indexes,
                        CoreIndex = pair.Key.coreIndex,
                        Terrain = pair.Value,
                        PopCount = pops[pair.Key]
                    };
                    return province;
                });

            return provinces;
        }

        internal static Dictionary<string, Province> Build(IEnumerable<MapCell> mapCells, Func<Province, IEnumerable<CentralArmy>> armyFinder)
        {
            var rslt = new Dictionary<string, Province>();

            var provinceId2Cells = mapCells.Where(x => x.ProvinceId != null)
                .GroupBy(x => x.ProvinceId)
                .ToDictionary(x => x.Key, y => (IEnumerable<MapCell>)y);

            foreach (var pair in provinceId2Cells)
            {
                var province = new Province(pair.Key, pair.Value, armyFinder);
                rslt.Add(province.Id, province);
            }

            BuilderNeighbors(rslt.Values, provinceId2Cells);

            return rslt;
        }

        //internal static IEnumerable<Province> Build(int high, int width, string seed)
        //{
        //    var blocks = BlockBuilder.Build(high, width, seed);
        //    var terrains = TerrainBuilder.Build(blocks, seed);
        //    var pops = PopCountBuilder.Build(terrains, seed);
        //}

        private static void BuilderNeighbors(IEnumerable<Province> provinces, Dictionary<string, IEnumerable<MapCell>> provinceId2Cells)
        {
            var provinceQueue = new Queue<Province>(provinces);
            while (provinceQueue.Count > 0)
            {
                var current = provinceQueue.Dequeue();
                var neighbors = provinceQueue.Where(other => IsNeighbor(provinceId2Cells[current.Id], provinceId2Cells[other.Id])).ToArray();

                current.Neighbors = current.Neighbors.Union(neighbors).ToArray();
                foreach (var neighbor in neighbors)
                {
                    neighbor.Neighbors = neighbor.Neighbors.Append(current).ToArray();
                }
            }
        }

        private static bool IsNeighbor(IEnumerable<MapCell> cellgroup1, IEnumerable<MapCell> cellgroup2)
        {
            foreach (var cell1 in cellgroup1)
            {
                foreach (var cell2 in cellgroup2)
                {
                    if (MapCell.IndexMethods.GetNeighborCells(cell1.Index).Values.Contains(cell2.Index))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
