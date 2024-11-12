using Godot;
using System;

public partial class SeedPanel : Panel
{
    [Signal]
    public delegate void ConfirmEventHandler(string seed);

    public Button ConfirmButton => GetNode<Button>("VBoxContainer/Button");
    public TextEdit TextEdit => GetNode<TextEdit>("VBoxContainer/Editor");

    public override void _Ready()
    {
        ConfirmButton.Connect(Button.SignalName.Pressed, Callable.From(() =>
        {
            EmitSignal(SignalName.Confirm, TextEdit.Text);
        }));
    }
}
