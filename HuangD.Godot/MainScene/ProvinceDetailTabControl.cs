using Godot;
using HuangD.Sessions;

public abstract partial class ProvinceDetailTabControl : Control
{
    internal abstract string TabName { get; }
    internal abstract void Update(Province province);
}