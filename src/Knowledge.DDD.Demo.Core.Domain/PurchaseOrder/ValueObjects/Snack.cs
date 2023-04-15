using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;

public sealed class Snack: ValueObject
{
    private Snack(SnackName snackName)
    {
        SnackName = snackName;
    }

    public SnackName SnackName { get; }

    public static Snack From(SnackName snackName) => new(snackName);
    
    public static Result<Snack> From(string snackNameValue)
    {
        var snackNameResult = SnackName.From(snackNameValue);
        return !snackNameResult.Succeeded 
            ? Result<Snack>.Fail(snackNameResult.Exception.EnsureNotNull()) 
            : Result<Snack>.Ok(From(snackNameResult.ResultValue.EnsureNotNull()));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SnackName;
    }
}