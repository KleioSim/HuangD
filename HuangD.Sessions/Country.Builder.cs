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
                var country = new Country(color);
                rslt.Add(country.Id, country);

                foreach (var province in provGroups)
                {
                    province.Owner = country;
                }

                country.CapitalProvince = provGroups.First();
            }

            foreach (var country in rslt.Values)
            {
                country.Name = GenerateCountryName(rslt.Values.Select(x => x.Name));
            }

            return rslt;
        }

        private static string[] Names = { "华", "同", "陇", "宁", "庆", "定", "夏", "丰", "宥", "陕", "虢", "许", "蔡", "陈", "宋", "濮", "徐", "宿", "齐", "晋", "辽", "岚", "魏", "卫", "邢", "赵", "冀", "祁", "顺", "燕", "崇", "黎", "昌", "信", "襄", "兴", "巴", "商", "金", "开", "邓", "唐", "隋", "郢", "夔", "楚", "寿", "蕲", "申", "安", "舒", "苏", "越", "明", "温", "建", "宣", "虔", "岳", "朗", "永", "郴", "邵", "辰", "鄯", "秦", "成", "兰", "凉", "甘", "伊", "汉", "彭", "蜀", "普", "荣", "嘉", "茂", "炎", "向", "冉", "笮", "柘", "广", "贺", "端", "新", "康", "封", "桂", "蒙", "罗", "雷", "肃", "葛", "英", "应", "岐", "恒", "宛", "尧", "舜", "禹", "雁", "延", "梁", "吴", "韩", "晏", "蓟", "竺" };

        private static string GenerateCountryName(IEnumerable<string> usedNames)
        {
            return Names.Except(usedNames).First();
        }
    }

}