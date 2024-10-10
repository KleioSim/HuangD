using DynamicData;
using Godot;
using HuangD.Godot.Utilties;
using System;
using System.Linq;

public partial class BaseMap : Node2D
{
    BlockMap BlockMap => GetNode<BlockMap>("BlockMap");
    TerrainMap TerrainMap => GetNode<TerrainMap>("TerrainMap");
    PopCountMap PopCountMap => GetNode<PopCountMap>("PopCountMap");
    ProvinceMap ProvinceMap => GetNode<ProvinceMap>("ProvinceMap");

    public override void _Ready()
    {
        var session = this.GetSession();

        foreach (var block in session.Blocks.Values)
        {
            BlockMap.AddOrUpdate(block.Indexes);
        }

        foreach (var pair in session.Block2Terrain)
        {
            TerrainMap.AddOrUpdate(session.Blocks[pair.Key].Indexes, pair.Value);
        }

        foreach (var province in session.Provinces.Values)
        {
            PopCountMap.AddOrUpdate(province.Block.Indexes, province.PopCount * 10 / session.Provinces.Values.Max(p => p.PopCount));
            ProvinceMap.AddOrUpdate(province.Block.Indexes, province.Id);
        }
    }

    internal Vector2 GetMapCenter()
    {
        return BlockMap.MapToLocal(BlockMap.GetUsedRect().GetCenter());
    }

    internal Vector2 GetProvinceCenter(string id)
    {
        var session = this.GetSession();

        var coreIndex = session.Provinces[id].Block.coreIndex;
        return ProvinceMap.MapToLocal(new Vector2I(coreIndex.X, coreIndex.Y));
    }

    internal string LocalToProvince(Vector2 vector2)
    {
        var cellVector = ProvinceMap.LocalToMap(vector2);

        for (int i = 0; i < ProvinceMap.GetLayersCount(); i++)
        {
            if (ProvinceMap.GetCellSourceId(i, cellVector) != -1)
            {
                return ProvinceMap.GetLayerName(i);
            }
        }

        return null;
    }
}