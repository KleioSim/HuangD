using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;

public partial class Command : ViewControl
{
    protected override void Initialize()
    {
        CommandConsole.IsVaild = true;

        var session = this.GetSession();
        ViewControl.OnMessage = session.OnMessage;

        CommandConsole.AddCommand("ChangeCounrtryOwner".ToLower(), ChangeProvinceOwner);
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
