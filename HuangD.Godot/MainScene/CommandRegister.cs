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
    public override void _Ready()
    {
        CommandConsole.IsVaild = true;

        var ass = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName.Contains("HuangD")).ToArray();
        var def = ass.SelectMany(x => x.DefinedTypes).ToList();

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName.Contains("HuangD"))
            .SelectMany(x => x.DefinedTypes)
            .Where(x => x.GetCustomAttribute<RegistCommandAttribute>() != null && x.IsAssignableTo(typeof(IMessage)))
            .ToArray();

        foreach (var type in types)
        {
            var constructor = type.GetConstructors().SingleOrDefault();

            CommandConsole.AddCommand(type.Name,
                (object[] parameters) =>
                {
                    var obj = constructor.Invoke(parameters) as IMessage;

                    this.GetSession().OnMessage(obj);
                },
                constructor.GetParameters().ToDictionary(k => k.Name, v => v.ParameterType));
        }
    }
}
