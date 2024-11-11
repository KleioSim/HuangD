using Godot;
using System;

public partial class EdgeMap : TileMapLayer
{
    internal void Update(Vector2I size)
    {
        this.Clear();

        for (int i = -1; i <= size.X; i++)
        {
            this.SetCell(new Vector2I(i, -1), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(i, size.Y), 0, Vector2I.Zero, 0);
        }

        for (int i = -1; i <= size.Y; i++)
        {
            this.SetCell(new Vector2I(-1, i), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(size.X, i), 0, Vector2I.Zero, 0);
        }


    }
}
