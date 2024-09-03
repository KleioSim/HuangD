using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : Control
{
    private IEnumerable<DetailPanel> viewContrls;

    public string EntityId
    {
        get
        {
            return viewContrls.Single(x => x.Visible).EntityId;
        }
        set
        {
            foreach (var control in viewContrls)
            {
                control.Visible = false;
            }

            switch (this.GetSession().Entities[EntityId])
            {
                case Province:
                    var provinceDetailPanel = viewContrls.OfType<ProvinceDetailPanel>().Single();
                    provinceDetailPanel.EntityId = value;
                    provinceDetailPanel.Visible = true;
                    break;
            }
        }
    }


    public override void _Ready()
    {
        viewContrls = new DetailPanel[]
        {
            GetNode<ProvinceDetailPanel>(""),
        };
    }
}