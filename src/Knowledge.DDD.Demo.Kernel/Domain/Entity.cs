namespace Knowledge.DDD.Demo.Kernel.Domain;

public abstract class BaseEntity<TId> : IEquatable<BaseEntity<TId>> where TId : notnull
{
    private readonly Queue<IDomainEvent> _events = new();
    
    protected BaseEntity(TId id) => Id = id;

    public TId Id { get; }

    public IEnumerable<IDomainEvent> DequeueEvents()
    {
        while (_events.Any())
        {
            var domainEvent = DequeueDomainEvent();
            yield return domainEvent;
        }
    }

    public override bool Equals(object? obj) => obj is BaseEntity<TId> entity && Id.Equals(entity.Id);

    public bool Equals(BaseEntity<TId>? other) => Equals((object?)other);

    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right) => Equals(left, right);

    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right) => !Equals(left, right);

    public override int GetHashCode() => Id.GetHashCode();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _events.Enqueue(domainEvent);

    private IDomainEvent DequeueDomainEvent() => _events.Dequeue();
}

public abstract class Entity : BaseEntity<Id>
{
    protected Entity(Id id) : base(id) { }
}

public abstract class Entity<TId> : BaseEntity<TId> where TId : notnull
{
    protected Entity(TId id) : base(id) { }
}