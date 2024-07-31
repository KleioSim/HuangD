using Chrona.Engine.Godot;
using Godot;
using System;

public partial class PoliticalInfo : Control
{
    public void OnZoomed(Vector2 zoom)
    {
        this.Scale = Vector2.One / zoom;
    }
}
