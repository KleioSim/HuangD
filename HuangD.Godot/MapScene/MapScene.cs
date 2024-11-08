using Godot;

public partial class MapScene : GraphEdit
{
    GraphElement GraphElement => GetNode<GraphElement>("GraphElement");
    BaseMap BaseMap => GetNode<BaseMap>("GraphElement/BaseMap");
    PoliticalContainer PoliticalContainer => GetNode<PoliticalContainer>("GraphElement/BaseMap/PoliticalContainer");

    public override void _Ready()
    {
        GraphElement.Size = BaseMap.GetSize();
        GraphElement.PositionOffset = this.GetViewportRect().Size / 2 - GraphElement.Size / 2;

        PoliticalContainer.BuildPoliticalInfos(BaseMap.GetProvinceCenter);
    }

    //public override void _UnhandledInput(InputEvent @event)
    //{
    //    if (@event is InputEventMouseButton eventKey)
    //    {
    //        if (eventKey.Pressed)
    //        {
    //            if (eventKey.ButtonIndex == MouseButton.Left)
    //            {
    //                var provinceId = BaseMap.LocalToProvince(GetGlobalMousePosition());

    //                if (provinceId != null)
    //                {
    //                    this.GetSelectEntity().Current = this.GetSession().Entities[provinceId];
    //                }
    //            }
    //        }
    //        return;
    //    }
    //}
}
