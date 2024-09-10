using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : PanelContainer
{

    public string EntityId
    {
        get
        {
            return entityId;
        }
        set
        {
            foreach (var control in FindViewContrls())
            {
                control.Visible = false;
            }

            entityId = value;
            switch (this.GetSession().Entities[entityId])
            {
                case Province:
                    var provinceDetailPanel = FindViewContrls().OfType<ProvinceDetailPanel>().Single();
                    provinceDetailPanel.EntityId = value;
                    provinceDetailPanel.Visible = true;
                    provinceDetailPanel.IsDirty = true;
                    break;
            }
        }
    }

    private string entityId;
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