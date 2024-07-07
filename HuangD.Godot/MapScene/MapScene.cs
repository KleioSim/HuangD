using DynamicData;
using Godot;
using HuangD.Sessions;
using System;
using System.Reactive.Linq;

public partial class MapScene : Control
{
    private IDisposable _disposed;

    private TerrainMap terrainMap => GetNode<TerrainMap>("TerrainMap");

    public override void _Ready()
    {
        var session = new Session();
        _disposed = session.Map.Terrains.Connect().Subscribe(changes =>
        {
            foreach (var change in changes)
            {
                switch (change.Reason)
                {
                    case ChangeReason.Add:
                        terrainMap.AddOrUpdate(change.Current.Index, change.Current.Type);
                        break;
                }
            }
        });
    }

    public override void _ExitTree()
    {
        _disposed.Dispose();
    }

    public override void _Process(double delta)
    {
       
    }
}
