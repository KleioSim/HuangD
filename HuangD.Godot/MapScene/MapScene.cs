using Godot;

public partial class MapScene : GraphEdit
{
    GraphElement GraphElement => GetNode<GraphElement>("GraphElement");
    BaseMap BaseMap => GetNode<BaseMap>("GraphElement/BaseMap");

    public override void _Ready()
    {
        var commandRegister = new CommandRegister();
        GetTree().Root.AddChild(commandRegister, true);

        GraphElement.Size = BaseMap.GetSize();
        GraphElement.PositionOffset = this.GetViewportRect().Size / 2 - GraphElement.Size / 2;
    }
}
