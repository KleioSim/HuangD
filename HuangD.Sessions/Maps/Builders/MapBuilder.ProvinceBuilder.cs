﻿

using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Sessions.Maps.Builders;

public static partial class MapBuilder
{
    internal class ProvinceBuilder
    {
        public static Dictionary<Index, string> Build(Dictionary<Index, int> popDict, string seed, int maxPopCount, int maxIndexCount)
        {
            var indexs = popDict.Keys.ToHashSet();

            var rslt = new Dictionary<Index, string>();

            var random = RandomBuilder.Build(seed);

            while (indexs.Count != 0)
            {
                var maxPopIndex = indexs.OrderBy(k => popDict[k])
                    .FirstOrDefault(x => MapCell.IndexMethods.GetNeighborCells(x).Values.Intersect(indexs).All(neighbor => indexs.Contains(neighbor)));

                if (maxPopIndex == null)
                {
                    throw new Exception();
                }

                indexs.Remove(maxPopIndex);


                var currentGroup = new HashSet<Index>() { maxPopIndex };

                var neighbors = GetNeighborsInDirects(maxPopIndex).Intersect(indexs).ToHashSet();

                bool isFull = false;
                while (neighbors.Count != 0)
                {
                    var next = neighbors.FirstOrDefault(x => GetNeighborsInDirects(x).Intersect(indexs).Count() <= 1);
                    if (next == null)
                    {
                        next = neighbors.OrderBy(x => popDict[x]).ElementAt(random.Next(0, neighbors.Count / 2));
                    }

                    currentGroup.Add(next);
                    indexs.Remove(next);
                    neighbors.Remove(next);

                    neighbors.UnionWith(GetNeighborsInDirects(next).Intersect(indexs));

                    if (currentGroup.Count >= maxIndexCount)
                    {
                        isFull = true;
                        break;
                    }
                    if (currentGroup.Sum(k => popDict[k]) >= maxPopCount)
                    {
                        isFull = true;
                        break;
                    }
                }

                string currentId = UUID.Generate("PROV");

                if (!isFull)
                {
                    foreach (var index in currentGroup.SelectMany(x => GetNeighborsInDirects(x)))
                    {
                        if (rslt.TryGetValue(index, out currentId))
                        {
                            break;
                        }
                    }
                }

                foreach (var index in currentGroup)
                {
                    rslt.Add(index, currentId);
                }
            }

            return rslt;
        }

        private static IEnumerable<Index> GetNeighborsInDirects(Index index)
        {

            var directions = new[] { Direction.LeftSide, Direction.RightSide, Direction.TopSide, Direction.BottomSide };
            return directions.Select(x => MapCell.IndexMethods.GetNeighborCell(index, x));
        }
    }
}