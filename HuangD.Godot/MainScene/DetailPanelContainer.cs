using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : Control
{

    public string EntityId
    {
        get
        {
            return FindViewContrls().Single(x => x.Visible).EntityId;
        }
        set
        {
            foreach (var control in FindViewContrls())
            {
                control.Visible = false;
            }

            switch (this.GetSession().Entities[EntityId])
            {
                case Province:
                    var provinceDetailPanel = FindViewContrls().OfType<ProvinceDetailPanel>().Single();
                    provinceDetailPanel.EntityId = value;
                    provinceDetailPanel.Visible = true;
                    break;
            }
        }
    }

    private IEnumerable<DetailPanel> viewContrls;

    private IEnumerable<DetailPanel> FindViewContrls()
    {
        viewContrls ??= new DetailPanel[]
        {
            GetNode<ProvinceDetailPanel>("VBoxContainer/ProvinceDetailPanel"),
        };

        return viewContrls;
    }
}