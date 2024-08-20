using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class MainScene : ViewControl
{
    Button NextTurn => GetNode<Button>("");
    MapScene MapScene => GetNode<MapScene>("/root/MapScene");
    InstancePlaceholder HolderDetailPanel => GetNode<InstancePlaceholder>("");

    protected override void Initialize()
    {
        NextTurn.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnNextTurn));


        MapScene.Connect(MapScene.SignalName.ClickProvince, new Callable(this, MethodName.OnSelectEntity));
        MapScene.Connect(MapScene.SignalName.ClickArmy, new Callable(this, MethodName.OnSelectEntity));
        MapScene.Connect(MapScene.SignalName.ClickArmyMoveTarget, new Callable(this, MethodName.OnStartArmyMove));
    }

    protected override void Update()
    {

    }

    private void OnNextTurn()
    {
        SendCommand(new Command_NextTurn());
    }

    private void OnSelectEntity(string id)
    {
        var detailPanel = HolderDetailPanel.CreateInstance() as DetailPanel;
        detailPanel.EntityId = id;

        MapScene.CleanMoveTargets();
        if (this.GetSession().Entities[id] is CentralArmy centralArmy)
        {
            MapScene.ShowMoveTargets(centralArmy.Position.Neighbors);
        }
    }

    private void OnStartArmyMove(string provinceId)
    {
        var detailPanel = HolderDetailPanel.GetParent().GetChildren().OfType<DetailPanel>().SingleOrDefault();

        if (this.GetSession().Entities[detailPanel.EntityId] is CentralArmy centralArmy)
        {
            SendCommand(new Command_ArmyMove(detailPanel.EntityId, provinceId));
        }
    }
}
