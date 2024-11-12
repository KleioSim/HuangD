using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BlockMap : Node2D
{
    private Random random = new System.Random();

    private TileMapLayer TileMapLayer => GetNode<TileMapLayer>("TileMapLayer");

    internal void Refresh()
    {
        Clear();

        foreach (var block in this.GetSession().Blocks.Values)
        {
            var newTileMapLayer = TileMapLayer.Duplicate() as TileMapLayer;
            TileMapLayer.AddSibling(newTileMapLayer);

            newTileMapLayer.Name = block.Id;
            newTileMapLayer.Modulate = new Color(random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f, random.Next(0, 10) / 10.0f);

            foreach (var index in block.Indexes)
            {
                newTileMapLayer.SetCell(new Vector2I(index.X, index.Y), 0, Vector2I.Zero, 0);
            }
        }
    }

    private void Clear()
    {
        TileMapLayer.Clear();

        var needRemoveItems = TileMapLayer.GetParent().GetChildren().OfType<TileMapLayer>().Where(x => x != TileMapLayer).ToArray();
        foreach (var item in needRemoveItems)
        {
            item.QueueFree();
        }
    }
}