using DynamicData;
using HuangD.Sessions.Maps.Builders;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Xml.Linq;

namespace HuangD.Sessions.Maps;

public partial class Map
{
    public static IIndexMethods IndexMethods { get; set; } = new SquareIndexMethods();

    public SourceCache<TerrainItem, Index> Terrains = new SourceCache<TerrainItem, Index>(item => item.Index);
    public SourceCache<PopItem, Index> Pops = new SourceCache<PopItem, Index>(item => item.Index);
    public SourceCache<ProvinceCell, Index> Provinces = new SourceCache<ProvinceCell, Index>(item => item.Index);

    //public void OnNext()
    //{
    //    var dict = Terrains.Items.ToDictionary(x=>x.Index, x => x);
    //    foreach (var key in dict.Keys)
    //    {


    //        var count = IndexMethods.GetNeighborCells(key).Values.Count(x => dict.ContainsKey(x) && dict[x].Type == TerrainType.Land);
    //        var arry = IndexMethods.GetNeighborCells(key).Values.Where(x => dict.ContainsKey(x)).ToArray();
    //        var item = dict[key];
    //        if (item.Type == TerrainType.Land)
    //        {
    //            if (count < 2)
    //            {
    //                Terrains.AddOrUpdate(new TerrainItem() { Index = key, Type = TerrainType.Water });
    //            }
    //        }
    //        else
    //        {
    //            if (count > 5)
    //            {
    //                Terrains.AddOrUpdate(new TerrainItem() { Index = key, Type = TerrainType.Land });
    //            }
    //        }
    //    }
    //}
}

public class MapCell
{
    public Index index { get; private init; }
    public int popCount { get; private init; }
    public string provinceId { get; private init; }
}

public class ProvinceCell
{
    public Index Index { get; set; }
    public string ProvinceId { get; set; }
}