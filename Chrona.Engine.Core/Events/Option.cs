using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Core.Sessions;

namespace Chrona.Engine.Core.Events;

public class Option : IOption
{
    public static Action<IMessage>? ProcessMessage;

    public string Tip => throw new NotImplementedException();

    public string Desc => throw new NotImplementedException();

    public IOptionDef Def { get; }

    private ProcessContext context;

    public Option(IOptionDef Def, ProcessContext context)
    {
        this.Def = Def;
        this.context = context;
    }

    public void Do()
    {
        if (ProcessMessage == null)
        {
            throw new Exception();
        }

        foreach (var message in Def.ProductMessage(context))
        {
            ProcessMessage(message);
        }
    }
}

