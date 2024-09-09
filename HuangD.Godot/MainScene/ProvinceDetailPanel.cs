using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class ProvinceDetailPanel : DetailPanel
{
    TabContainer TabContainer => GetNode<TabContainer>("TabContainer");

    protected override void Initialize()
    {
        TabContainer.Connect(TabContainer.SignalName.TabChanged, Callable.From((long index) =>
        {
            Update();
        }));

        for (int i = 0; i < TabContainer.GetTabCount(); i++)
        {
            var tabControl = TabContainer.GetTabControl(i) as ProvinceDetailTabControl;
            TabContainer.SetTabTitle(i, tabControl.TabName);
        }
    }

    private void TabContainer_TabChanged(long tab)
    {
        throw new NotImplementedException();
    }

    protected override void Update()
    {
        var province = this.GetSession().Entities[EntityId] as Province;
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
