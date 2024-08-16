using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions;

public partial class Province
{
    internal static class Builder
    {
        internal static Dictionary<string, Province> Build(IEnumerable<MapCell> mapCells, Func<Province, IEnumerable<CentralArmy>> armyFinder)
        {
            var rslt = new Dictionary<string, Province>();

            var provinceId2Cells = mapCells.Where(x => x.ProvinceId != null)
                .GroupBy(x => x.ProvinceId)
                .ToDictionary(x => x.Key, y => (IEnumerable<MapCell>)y);

            foreach (var pair in provinceId2Cells)
            {
                var province = new Province(pair.Key, pair.Value, armyFinder);
                rslt.Add(province.Key, province);
            }

            BuilderNeighbors(rslt.Values, provinceId2Cells);

            return rslt;
        }

        private static void BuilderNeighbors(IEnumerable<Province> provinces, Dictionary<string, IEnumerable<MapCell>> provinceId2Cells)
        {
            var provinceQueue = new Queue<Province>(provinces);
            while (provinceQueue.Count > 0)
            {
                var current = provinceQueue.Dequeue();
                var neighbors = provinceQueue.Where(other => IsNeighbor(provinceId2Cells[current.Key], provinceId2Cells[other.Key])).ToArray();

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
