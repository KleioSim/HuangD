using Godot;
using HuangD.Godot.Utilties;
using System;

public partial class EdgeMap : TileMapLayer
{
    internal void Refresh(Vector2 scale)
    {
        this.Clear();

        var size = new Vector2I(this.GetSession().MapSize.x, this.GetSession().MapSize.y) * scale;
        for (int i = 0; i <= size.X+1; i++)
        {
            this.SetCell(new Vector2I(i, 0), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I(i, (int)size.Y+1), 0, Vector2I.Zero, 0);
        }

        for (int i = 0; i <= size.Y+1; i++)
        {
            this.SetCell(new Vector2I(0, i), 0, Vector2I.Zero, 0);
            this.SetCell(new Vector2I((int)size.X+1, i), 0, Vector2I.Zero, 0);
        }

    }
}
