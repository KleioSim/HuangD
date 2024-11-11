using Godot;
using HuangD.Godot.Utilties;
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

    internal void Refresh()
    {
        Clear();


        foreach (var pair in this.GetSession().Block2Terrain)
        {
            var block = this.GetSession().Blocks[pair.Key];
            foreach (var index in block.Indexes)
            {
                terrain2layer[pair.Value].SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
            }
        }
    }

    private void Clear()
    {
        foreach (var layer in terrain2layer.Values)
        {
            layer.Clear();
        }
    }
}