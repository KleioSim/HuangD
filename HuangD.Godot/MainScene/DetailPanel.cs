using Godot;
using System;

public partial class DetailPanel : Panel
{
    public Label label => GetNode<Label>("Label");

    public string EntityId
    {
        get => entityId;
        set
        {
            label.Text = value;
            entityId = value;
        }
    }

    private string entityId;

    public override void _Ready()
    {

    }
}
