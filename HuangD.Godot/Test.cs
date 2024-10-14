using Chrona.Engine.Godot;
using Chrona.Engine.Godot.UBBCodes;
using Godot;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public partial class Test : Control
{
    public static IData data;
    Label label => GetNode<Label>("Label");

    public override void _Ready()
    {
        data = Decorator<IData>.Create(new Data());
    }

    public void OnButton()
    {
        data.SetName(Guid.NewGuid().ToString());
    }

    public void OnRemove()
    {
        label.QueueFree();
    }
}


public interface IData
{
    string GetName();
    void SetName(string name);
}

public class Data : IData
{
    private string name;

    public string GetName()
    {
        return name;
    }

    [DataChange]
    public void SetName(string value)
    {
        name = value;
    }
}

