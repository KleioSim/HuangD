using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : PanelContainer, IView<ISessionData>
{
    private IEnumerable<DetailPanel> viewContrls;

    private IEnumerable<DetailPanel> FindViewContrls()
    {
        viewContrls ??= new DetailPanel[]
        {
            GetNode<ProvinceDetailPanel>("VBoxContainer/ProvinceDetailPanel"),
        };

        return viewContrls;
    }

    public override void _Ready()
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        foreach (var control in FindViewContrls())
        {
            control.Visible = false;
        }

        switch (this.GetSession().SelectedEntity)
        {
            case Province:
                var provinceDetailPanel = FindViewContrls().OfType<ProvinceDetailPanel>().Single();
                provinceDetailPanel.Visible = true;
                break;
        }
    }
}