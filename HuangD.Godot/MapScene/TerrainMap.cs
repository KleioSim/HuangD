using Godot;
using HuangD.Sessions.Maps;
using System.Collections.Generic;

public partial class TerrainMap : TileMap
{
    private Dictionary<TerrainType, int> layerIds = new Dictionary<TerrainType, int>()
    {
        { TerrainType.Water, 0 },
        { TerrainType.Land, 1},
        { TerrainType.Hill, 2 },
        { TerrainType.Mount, 3 },
        //{ TerrainType.Steppe, 4 },
    };

    private Dictionary<TerrainType, Color> colors = new Dictionary<TerrainType, Color>()
    {
        { TerrainType.Water, new Color(){ R = 0, G = 0, B = 1, A = 1} },
        { TerrainType.Land, new Color(){ R = 0, G = 1, B = 0, A = 1} },
        { TerrainType.Hill, new Color(){ R = 0, G = 0.5f, B = 0.5f, A = 1} },
        { TerrainType.Mount, new Color(){ R = 0.5f, G = 0, B = 0.5f, A = 1} },
        //{ TerrainType.Steppe, new Color(){ R = 0, G = 0, B = 1, A = 1} },
    };

    private Dictionary<TerrainType, int> sourceIds = new Dictionary<TerrainType, int>()
    {
        { TerrainType.Water, 0 },
        { TerrainType.Land, 1 },
        { TerrainType.Hill, 2 },
        { TerrainType.Mount, 3 },
    };

    public override void _Ready()
    {

    }

    public void AddOrUpdate(Index index, TerrainType type)
    {
        //foreach(var id in layerIds.Values)
        //{
        //    this.EraseCell(id, new Vector2I(index.X, index.Y));
        //};

        //this.SetCell(layerIds[type], new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);


        this.EraseCell(0, new Vector2I(index.X, index.Y));
        this.SetCell(0, new Vector2I(index.X, index.Y), sourceIds[type], Vector2I.Zero, 0);
    }
}