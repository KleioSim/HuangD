using Godot;
using HuangD.Godot.Utilties;
using System.Linq;

public partial class BaseMap : Node2D
{
    BlockMap BlockMap => GetNode<BlockMap>("BlockMap");
    TerrainMap TerrainMap => GetNode<TerrainMap>("TerrainMap");
    PopCountMap PopCountMap => GetNode<PopCountMap>("PopCountMap");
    ProvinceMap ProvinceMap => GetNode<ProvinceMap>("ProvinceMap");

    public void InitMap()
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

        foreach (var province in session.Provinces)
        {
            PopCountMap.AddOrUpdate(province.Block.Indexes, province.PopCount * 10 / session.Provinces.Max(p => p.PopCount));
            ProvinceMap.AddOrUpdate(province.Block.Indexes, province.Id);
        }
    }

    internal Vector2 GetCenterPosition()
    {
        return BlockMap.MapToLocal(BlockMap.GetUsedRect().GetCenter());
    }
}