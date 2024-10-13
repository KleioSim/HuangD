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

[AttributeUsage(AttributeTargets.Method)]
public class DataChangeAttribute : Attribute
{

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

public class Decorator<T> : DispatchProxy
{
    public static uint Label { get; private set; }
    private T _decorated;

    private Dictionary<MethodInfo, bool> method2DataChangeFlag = new Dictionary<MethodInfo, bool>();

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        if (!method2DataChangeFlag.TryGetValue(targetMethod, out var flag))
        {
            var methodParameterTypes = targetMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            var realMethodInfo = _decorated.GetType().GetMethod(targetMethod.Name, methodParameterTypes);

            var attribute = realMethodInfo.GetCustomAttribute<DataChangeAttribute>();
            flag = attribute != null;

            method2DataChangeFlag.Add(targetMethod, flag);
        }

        if (flag)
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