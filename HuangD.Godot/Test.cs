using Chrona.Engine.Godot;
using Godot;
using HuangD.Sessions;
using System;
using System.Collections.Generic;
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



public interface IView
{
    static Dictionary<IView, uint> view2Label = new Dictionary<IView, uint>();

    bool IsDirty()
    {
        if (!view2Label.TryGetValue(this, out uint value))
        {
            view2Label.Add(this, 0);
            var node = this as Node;
            node.Connect(Node.SignalName.TreeExiting, Callable.From(() => view2Label.Remove(this)));
            return false;
        }

        view2Label[this] = Decorator<IData>.Label;
        return value != Decorator<IData>.Label;
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

    public void SetName(string value)
    {
        name = value;
    }
}

public class Decorator<T> : DispatchProxy
{
    public static uint Label { get; private set; }
    private T _decorated;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        if (targetMethod.Name == nameof(Data.SetName))
        {
            Label++;
        }


        var result = targetMethod.Invoke(_decorated, args);

        return result;
    }

    public static T Create(T decorated)
    {
        object proxy = Create<T, Decorator<T>>();
        ((Decorator<T>)proxy).SetParameters(decorated);

        return (T)proxy;
    }

    private void SetParameters(T decorated)
    {
        if (decorated == null)
        {
            throw new ArgumentNullException(nameof(decorated));
        }
        _decorated = decorated;
    }
}