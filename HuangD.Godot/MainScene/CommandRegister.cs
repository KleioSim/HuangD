using Chrona.Engine.Core.Interfaces;
using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class CommandRegister : Node
{
    public Action<IMessage> SendCommand;

    public override void _Ready()
    {
        CommandConsole.IsVaild = true;



        //CommandConsole.AddCommand("ChangeCounrtryOwner".ToLower(), ChangeProvinceOwner);


        Type type = typeof(Command_ChangeProvinceOwner);
        var constructor = type.GetConstructors().FirstOrDefault();

        CommandConsole.AddCommand(type.Name,
            (object[] parameters) =>
            {
                var obj = constructor.Invoke(parameters) as IMessage;

                SendCommand(obj);
            },
            constructor.GetParameters().ToDictionary(k => k.Name, v => v.ParameterType));
    }


    //void ChangeProvinceOwner(string provinceId, string countryId)
    //{
    //    var message = new Command_ChangeProvinceOwner(provinceId, countryId);
    //    ViewControl.SendCommand(message);
    //}
}
