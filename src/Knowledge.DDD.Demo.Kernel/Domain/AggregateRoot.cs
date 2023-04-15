namespace Knowledge.DDD.Demo.Kernel.Domain;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Id id) : base(id)
    {
    }
}

public abstract class AggregateRoot<TId> : Entity<Id<TId>> where TId : notnull
{
    protected AggregateRoot(Id<TId> id) : base(id)
    {
    }
}