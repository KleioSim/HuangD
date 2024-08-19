using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;
using System.Reflection;

public partial class CommandRegister : Node
{
    public Action<IMessage> SendCommand;

    public override void _Ready()
    {
        CommandConsole.IsVaild = true;


        var types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.GetCustomAttribute<RegistCommandAttribute>() != null && x.IsAssignableTo(typeof(IMessage)))
            .ToArray();
        ;

        foreach (var type in types)
        {
            var constructor = type.GetConstructors().SingleOrDefault();

            CommandConsole.AddCommand(type.Name,
                (object[] parameters) =>
                {
                    var obj = constructor.Invoke(parameters) as IMessage;

                    SendCommand(obj);
                },
                constructor.GetParameters().ToDictionary(k => k.Name, v => v.ParameterType));
        }
    }
}
