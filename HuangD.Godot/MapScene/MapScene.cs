using Godot;

public partial class MapScene : GraphEdit
{
    GraphElement GraphElement => GetNode<GraphElement>("GraphElement");
    BaseMap BaseMap => GetNode<BaseMap>("GraphElement/Control/BaseMap");

    public override void _Ready()
    {
        GraphElement.PositionOffset = this.GetViewportRect().Size / 2 - GraphElement.Size / 2;
        BaseMap.Position = GraphElement.Size / 2 - BaseMap.GetSize()/2;
    }
}
