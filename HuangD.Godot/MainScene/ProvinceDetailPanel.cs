using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class ProvinceDetailPanel : DetailPanel, IView<ISessionData>
{
    TabContainer TabContainer => GetNode<TabContainer>("TabContainer");

    public override void _Ready()
    {
        TabContainer.Connect(TabContainer.SignalName.TabChanged, Callable.From((long index) =>
        {
            var view = this as IView<ISessionData>;
            view.IsSelfDirty = true;
        }));

        for (int i = 0; i < TabContainer.GetTabCount(); i++)
        {
            var tabControl = TabContainer.GetTabControl(i) as ProvinceDetailTabControl;
            TabContainer.SetTabTitle(i, tabControl.TabName);
        }
    }

    public override void _Process(double delta)
    {
        var view = this as IView<ISessionData>;
        if (!view.IsDirty()) { return; }

        var province = this.GetSession().SelectedEntity as Province;
        Title.Text = province.Id;

        var control = TabContainer.GetCurrentTabControl();
        switch (control)
        {
            case ProvinceAttributeInfo attributeInfo:
                attributeInfo.Update(province);
                break;
            case ProvinceBattleInfo battleInfo:
                battleInfo.Update(province);
                break;
        }
    }
}
