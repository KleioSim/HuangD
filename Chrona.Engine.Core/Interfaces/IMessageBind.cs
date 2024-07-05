namespace Chrona.Engine.Core.Interfaces;

public interface IMessageBind
{
    public Type MessageType { get; }

    public DataVisitor TargetVisitor { get; }
    public DataVisitor ValueVisitor { get; }
}