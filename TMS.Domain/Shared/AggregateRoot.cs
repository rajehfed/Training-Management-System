using TMS.Domain.Shared.Interfaces;

namespace TMS.Domain.Shared;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    
    private readonly List<IDomainEvent> _domainEvents = [];
    protected AggregateRoot(TId id) : base(id) { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    protected void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
