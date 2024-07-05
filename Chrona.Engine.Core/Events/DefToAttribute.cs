namespace Chrona.Engine.Core.Events;

public class DefToAttribute : Attribute
{
    public readonly Type toType;
    public DefToAttribute(Type type)
    {
        toType = type;
    }
}