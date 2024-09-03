using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;

public partial class ProvinceDetailPanel : DetailPanel
{
    TabContainer TabContainer { get; set; }

    protected override void Initialize()
    {
        //TabContainer.Connect(TabContainer.SignalName.TabChanged, new Callable())
    }

    protected override void Update()
    {
        var province = this.GetSession().Entities[EntityId] as Province;
        Title.Text = province.Id;

        var control = TabContainer.GetCurrentTabControl();
        switch (control)
        {
            case ProvinceAttributeInfo attributeInfo:
                UpdateProvinceAttributeInfo(attributeInfo);
                break;
            case ProvinceBattleInfo battleInfo:
                UpdateProvinceBattleInfo(battleInfo);
                break;
        }
    }

    private void UpdateProvinceBattleInfo(ProvinceBattleInfo battleInfo)
    {
        throw new NotImplementedException();
    }

    private void UpdateProvinceAttributeInfo(ProvinceAttributeInfo attributeInfo)
    {
        throw new NotImplementedException();
    }
}

public partial class ProvinceAttributeInfo : Control
{

}

public partial class ProvinceBattleInfo : Control
{

}