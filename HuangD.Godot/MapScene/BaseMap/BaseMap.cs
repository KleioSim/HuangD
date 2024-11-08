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
    BoundaryMap BoundaryMap => GetNode<BoundaryMap>("BoundaryMap");
    EdgeMap EdgeMap => GetNode<EdgeMap>("EdgeMap");

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

        ProvinceMap.Clear();
        foreach (var province in session.Provinces.Values)
        {
            PopCountMap.AddOrUpdate(province.Block.Indexes, province.PopCount * 10 / session.Provinces.Values.Max(p => p.PopCount));
            ProvinceMap.AddOrUpdate(province.Block.Indexes, province.Id);
        }

        var mapSize = new Vector2I(session.MapSize.x, session.MapSize.y);
        var provTileSize = ProvinceMap.TileSize;

        var boundaryTileSize = BoundaryMap.TileSet.TileSize;
        BoundaryMap.Update(mapSize * provTileSize / boundaryTileSize, ProvinceMap);

        var edgeTileSize = EdgeMap.TileSet.TileSize;
        EdgeMap.Update(mapSize * provTileSize / edgeTileSize);
    }

    internal Vector2 GetProvinceCenter(string id)
    {
        var session = this.GetSession();

        var coreIndex = session.Provinces[id].Block.coreIndex;
        return ProvinceMap.MapToLocal(new Vector2I(coreIndex.X, coreIndex.Y));
    }

    internal Vector2 GetSize()
    {
        return EdgeMap.MapToLocal(EdgeMap.GetUsedRect().Size);
    }
}