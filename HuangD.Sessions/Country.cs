using HuangD.Sessions.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HuangD.Sessions;

public partial class Country
{
    public static class Builder
    {
        public static Dictionary<string, Country> Build(IEnumerable<Province> provinces, int maxPopCount, int maxProvCount, string seed)
        {
            var array = new float[] { 0.2f, 0.4f, 0.6f, 0.8f };

            var colors = new List<(float, float, float)>();
            for(int i=0; i<4; i++)
            {
                for (int j=0; j<4; j++)
                {
                    for(int  k=0; k<4; k++)
                    {
                        colors.Add((array[i], array[j], array[k]));
                    }
                }
            }

            Country.GetProvinces = (coutry) => provinces.Where(x => x.Owner == coutry);

            var random = RandomBuilder.Build(seed);

            var rslt = new Dictionary<string, Country>();

            var list = provinces.OrderBy(x => x.PopCount).ToList();
            while (list.Count != 0)
            {
                var firstProv = list[0];
                list.Remove(firstProv);

                var provGroups = new List<Province>() { firstProv };
                while (true)
                {
                    if (list.Count == 0)
                    {
                        break;
                    }
                    if (provGroups.Count >= maxProvCount)
                    {
                        break;
                    }
                    if (provGroups.Sum(x => x.PopCount) >= maxPopCount)
                    {
                        break;
                    }

                    var neighbors = provGroups.SelectMany(x => x.Neighbors)
                        .Where(x => list.Contains(x))
                        .ToArray();
                    if (neighbors.Length == 0)
                    {
                        break;
                    }

                    var newProv = neighbors[random.Next(0, neighbors.Length)];
                    provGroups.Add(newProv);
                    list.Remove(newProv);
                }

                var country = new Country(UUID.Generate("CNT"), colors[rslt.Count]);
                rslt.Add(country.Key, country);

                foreach (var province in provGroups)
                {
                    province.Owner = country;
                }
            }

            return rslt;
        }
    }

}