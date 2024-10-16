using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : PanelContainer, IView
{
    private IEnumerable<DetailPanel> viewContrls;
    private Control Content => GetNode<Control>("Content");
    private Button Button => GetNode<Button>("Content/PanelContainer/Title/Button");

    public override void _Ready()
    {
        Button.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            Content.Visible = false;
        }));
    }

    private IEnumerable<DetailPanel> FindViewContrls()
    {
        viewContrls ??= new DetailPanel[]
        {
            GetNode<ProvinceDetailPanel>("Content/ProvinceDetailPanel"),
        };

        return viewContrls;
    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        Content.Visible = this.GetSession().SelectedEntity != null;
        if(Content.Visible == false) { return; }

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