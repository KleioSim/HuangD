namespace Chrona.Engine.Core.Interfaces;

public interface IEntity
{
    bool IsInteractionDateOut { get; set; }
    IEnumerable<IInteraction> Interactions { get; }
}