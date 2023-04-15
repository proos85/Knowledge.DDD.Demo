using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Kernel.Domain;

public class Id : ValueObject
{
    private Id(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static Id New() => new(Guid.NewGuid());

    public static Result<Id> From(Guid value) =>
        value == Guid.Empty 
            ? Result<Id>.Fail<ArgumentException>($"Invalid id: {value}") 
            : Result<Id>.Ok(new Id(value));

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public abstract class Id<TId> : ValueObject where TId : notnull
{
    protected Id(TId value)
    {
        Value = value;
    }

    public TId Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}