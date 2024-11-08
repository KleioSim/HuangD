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
}
