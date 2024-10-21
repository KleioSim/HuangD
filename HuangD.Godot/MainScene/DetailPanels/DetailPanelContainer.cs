using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public partial class DetailPanelContainer : PanelContainer, IView
{
    private InstancePlaceholder ProvinceDetailPlaceholder => GetNode<InstancePlaceholder>("ProvinceDetailPanel");
    private InstancePlaceholder PlayerArmyDetailPlaceholder => GetNode<InstancePlaceholder>("PlayerArmyDetailPanel");

    public override void _Ready()
    {

    }

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        switch (this.GetSelectEntity().Current)
        {
            case Province:
                if(!ProvinceDetailPlaceholder.GetParent().GetChildren().OfType<ProvinceDetailPanel>().Any()) 
                {
                    ProvinceDetailPlaceholder.CreateInstance();
                }
                break;
            case PlayerArmyData:
                if (!PlayerArmyDetailPlaceholder.GetParent().GetChildren().OfType<PlayerArmyDetailPanel>().Any())
                {
                    PlayerArmyDetailPlaceholder.CreateInstance();
                }
                break;
        }
    }
}