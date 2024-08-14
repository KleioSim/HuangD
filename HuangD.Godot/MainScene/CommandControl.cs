using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class CommandControl : ViewControl
{
    protected override void Initialize()
    {
        CommandConsole.IsVaild = true;

        ViewControl.OnMessage = (msg) => this.GetSession().OnMessage(msg);

        CommandConsole.AddCommand("ChangeCounrtryOwner".ToLower(), ChangeProvinceOwner);


        //Type type = typeof(Command_ChangeProvinceOwner);

        //var constructor = type.GetConstructors().FirstOrDefault();

        //constructor.GetParameters();
    }

    protected override void Update()
    {

    }

    void ChangeProvinceOwner(string provinceId, string countryId)
    {
        var message = new Command_ChangeProvinceOwner(provinceId, countryId);
        ViewControl.SendCommand(message);
    }
}
