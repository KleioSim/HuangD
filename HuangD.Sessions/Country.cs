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

            var random = RandomBuilder.Build(seed);

            var colors = Enumerable.Range(0, 33).Select(x => x * 0.03f).OrderBy(_ => random.Next(0, 100)).ToArray();

            Country.GetProvinces = (coutry) => provinces.Where(x => x.Owner == coutry);


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

                var color = (colors[rslt.Count % colors.Length], ((rslt.Count % 3) + 1) * 0.33f, 1f);
                var country = new Country(UUID.Generate("CNTY"), color);
                rslt.Add(country.Key, country);

                foreach (var province in provGroups)
                {
                    province.Owner = country;
                }

                country.CapitalProvince = provGroups.First();
            }

            return rslt;
        }
    }

}