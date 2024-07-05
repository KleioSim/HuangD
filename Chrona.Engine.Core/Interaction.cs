using Chrona.Engine.Core.Interfaces;

namespace Chrona.Engine.Core;

public class Interaction : IInteraction
{
    public static Action<IMessage>? ProcessMessage;

    public string Desc => def.GetDesc(owner);

    private readonly IInteractionDef def;
    private readonly IEntity owner;


    public Interaction(IInteractionDef def, IEntity owner)
    {
        this.def = def;
        this.owner = owner;
    }

    public void Invoke(ISession session)
    {
        owner.IsInteractionDateOut = true;

        foreach (var message in def.Invoke(owner, session))
        {
            ProcessMessage(message);
        }
    }

    public IEnumerable<(bool flag, string desc)> GetVaildGroups(ISession session)
    {
        return def.GetVaildGroups(owner, session);
    }
}