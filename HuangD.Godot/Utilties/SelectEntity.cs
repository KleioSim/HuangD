using Chrona.Engine.Core;
using Godot;
using System;

public partial class SelectEntity : Node
{
    public object Current 
    {
        get => _current;
        set
        {
            Decorator.Label++;
            _current = value;
        }
    }

    private object _current;
}
