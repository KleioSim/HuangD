using Chrona.Engine.Godot;
using Chrona.Engine.Godot.Utilties;
using DynamicData;
using Godot;
using HuangD.Sessions;
using System;

namespace HuangD.Godot.Utilties;

static class NodeExtentions
{
    public static Session GetSession(this Node node)
    {
        var chroncle = node.GetNode<Global>("/root/Chrona_Global").Chroncle;
        return chroncle.Session as Session;
    }

    public static void SetSession(this Node node, Session session)
    {
        var chroncle = node.GetNode<Global>("/root/Chrona_Global").Chroncle;
        chroncle.Session = new Session();
    }

    public static IDisposable Subscribe<TObject, TKey>(this IObservable<IChangeSet<TObject, TKey>> observable,
        Action<TObject> onAdd,
        Action<TObject> onRemove,
        Action<TObject, TObject> onUpdate)
    {
        var dispose = observable.Subscribe(changes =>
        {
            foreach (var change in changes)
            {
                switch (change.Reason)
                {
                    case ChangeReason.Add:
                        onAdd?.Invoke(change.Current);
                        break;
                    case ChangeReason.Remove:
                        onRemove?.Invoke(change.Current);
                        break;
                    case ChangeReason.Update:
                        onUpdate?.Invoke(change.Current, change.Previous.Value);
                        break;
                }
            }
        });

        return dispose;
    }
}
