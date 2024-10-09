using Godot;
using HuangD.Sessions;
using HuangD.Sessions.Maps.Builders;
using System.Linq;

public partial class MapDebug : Node2D
{
    BlockMap BlockMap => GetNode<BlockMap>("BaseMap/BlockMap");
    TerrainMap TerrainMap => GetNode<TerrainMap>("BaseMap/TerrainMap");
    PopMap PopMap => GetNode<PopMap>("BaseMap/PopMap");
    ProvinceMap ProvinceMap => GetNode<ProvinceMap>("BaseMap/ProvinceMap");
    public override void _Ready()
    {
        var seed = "test";

        var blocks = MapBuilder.BlockBuilder.Build(120, 120, seed);

        foreach (var block in blocks)
        {
            BlockMap.AddOrUpdate(block.Indexes);
        }

        var block2Terrain = MapBuilder.TerrainBuilder.Build(blocks, seed);
        foreach (var pair in block2Terrain)
        {
            foreach (var index in pair.Key.Indexes)
            {
                TerrainMap.AddOrUpdate(index, pair.Value);
            }
        }

        var provinces = Province.Builder.Build(block2Terrain, seed);
        foreach (var province in provinces.Values)
        {
            PopMap.AddOrUpdate(province.Indexes, province.PopCount * 10 / provinces.Values.Max(p => p.PopCount));

            foreach (var index in province.Indexes)
            {
                ProvinceMap.AddOrUpdate(index, province.Id);
            }
        }
    }
}