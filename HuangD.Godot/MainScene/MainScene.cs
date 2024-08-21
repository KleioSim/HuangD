using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using HuangD.Sessions.Messages;
using System;
using System.Linq;

public partial class MainScene : ViewControl
{
    Button NextTurn => GetNode<Button>("CanvasLayer/NextTurn/Button");
    MapScene MapScene => GetNode<MapScene>("/root/MapScene");
    InstancePlaceholder DetailPanelPlaceHolder => GetNode<InstancePlaceholder>("CanvasLayer/DetailPanel");

    protected override void Initialize()
    {
        NextTurn.Connect(Button.SignalName.Pressed, new Callable(this, MethodName.OnNextTurn));

        MapScene.Connect(MapScene.SignalName.ClickEnity, new Callable(this, MethodName.OnSelectEntity));
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
        var detailPanel = DetailPanelPlaceHolder.GetParent().GetChildren().OfType<DetailPanel>().SingleOrDefault();
        if (detailPanel == null)
        {
            detailPanel = DetailPanelPlaceHolder.CreateInstance() as DetailPanel;
        }

        detailPanel.EntityId = id;
    }

    private void OnStartArmyMove(string provinceId)
    {
        var detailPanel = DetailPanelPlaceHolder.GetParent().GetChildren().OfType<DetailPanel>().Single();

        if (this.GetSession().Entities[detailPanel.EntityId] is CentralArmy centralArmy)
        {
            SendCommand(new Command_ArmyMove(detailPanel.EntityId, provinceId));

            MapScene.UpdateMoveInfo(detailPanel.EntityId);
        }
    }
}
