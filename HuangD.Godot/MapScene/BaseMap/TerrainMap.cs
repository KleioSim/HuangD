using Godot;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TerrainMap : Node2D
{
    private Dictionary<TerrainType, TileMapLayer> terrain2layer;

    public override void _Ready()
    {
        terrain2layer = GetChildren().OfType<TileMapLayer>().ToDictionary(k => Enum.Parse<TerrainType>(k.Name), v => v);
    }

    internal void AddOrUpdate(IEnumerable<HuangD.Sessions.Maps.Index> indexes, TerrainType terrainType)
    {
        foreach (var index in indexes)
        {
            terrain2layer[terrainType].SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
        }
    }
}