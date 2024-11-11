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
    PolitcalMap PolitcalMap => GetNode<PolitcalMap>("PolitcalMap");

    public override void _Ready()
    {
        Refresh();
    }

    internal Vector2 GetSize()
    {
        return EdgeMap.MapToLocal(EdgeMap.GetUsedRect().Size);
    }

    internal void Refresh()
    {
        BlockMap.Refresh();
        TerrainMap.Refresh();
        PopCountMap.Refresh();
        ProvinceMap.Refresh();
        PolitcalMap.Refresh();
        BoundaryMap.Refresh(ProvinceMap.TileSize / BoundaryMap.TileSet.TileSize);
        EdgeMap.Refresh((Vector2)ProvinceMap.TileSize / EdgeMap.TileSet.TileSize);
    }
}