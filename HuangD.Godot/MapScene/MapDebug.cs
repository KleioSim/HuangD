using Godot;
using HuangD.Sessions.Maps;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using HuangD.Sessions.Maps.Builders;
using System;
using static HuangD.Sessions.Maps.Builders.MapBuilder;

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

        foreach(var block in blocks)
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

        var block2PopCount = PopCountBuilder.Build(block2Terrain, seed);

        foreach (var pair in block2PopCount)
        {
            PopMap.AddOrUpdate(pair.Key.Indexes, pair.Value*10/ block2PopCount.Values.Max());
        }

        var provinces = Province.Builder.Build(block2Terrain, block2PopCount, seed);
        foreach (var province in provinces.Values)
        {
            foreach (var index in province.Indexes)
            {
                ProvinceMap.AddOrUpdate(index, province.Id);
            }
        }
    }
}