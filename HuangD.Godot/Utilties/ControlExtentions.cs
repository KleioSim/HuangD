using Godot;

namespace HuangD.Godot.Utilties;

internal static class ControlExtentions
{
    public static Vector2 GetLocalPositionWithPivotOffset(this Control control)
    {
        return control.Position + control.PivotOffset;
    }

    public static Vector2 GetGlobalPositionWithPivotOffset(this Control control)
    {
        return control.GlobalPosition + control.PivotOffset;
    }

    public static void SetLocalPositionWithPivotOffset(this Control control, Vector2 position)
    {
        control.Position = position - control.PivotOffset;
    }

    public static void SetGlobalPositionWithPivotOffset(this Control control, Vector2 position)
    {

        control.GlobalPosition = position - control.PivotOffset;
    }
}
