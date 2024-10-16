using Chrona.Engine.Godot;
using Godot;
using HuangD.Godot.Utilties;
using HuangD.Sessions;
using System;
using System.Linq;

public partial class ArrowContainer : Control, IView
{
    InstancePlaceholder AmryMoveArrowPlaceHolder => GetNode<InstancePlaceholder>("ArmyMoveArrow");

    public override void _Process(double delta)
    {
        var view = this as IView;
        if (!view.IsDirty()) { return; }

        var arrow = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().SingleOrDefault();

        var selectEntity = this.GetSession().SelectedEntity;
        if (selectEntity is Army army)
        {
            if (arrow == null)
            {
                arrow = AmryMoveArrowPlaceHolder.CreateInstance() as AmryMoveArrow;
            }

            arrow.ArmyId = army.Id;
            arrow.Visible = true;
        }
        else
        {
            arrow.QueueFree();
        }

    }

    internal void OnCameraZoom(Vector2 vector)
    {
        var arrows = AmryMoveArrowPlaceHolder.GetParent().GetChildren().OfType<AmryMoveArrow>().ToList();
        foreach (var arrow in arrows)
        {
            arrow.OnZoom(vector);
        }
    }
}