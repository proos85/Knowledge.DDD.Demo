using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;

public sealed class Money : ValueObject
{
    private Money(int euroCent)
    {
        // For simplicity currency is always EuroOnly
        TotalEuroCent = euroCent;
    }

    public int TotalEuroCent { get; }

    public int EuroOnly => TotalEuroCent / 100;

    public int CentOnly => TotalEuroCent - EuroOnly * 100;

    public static Money Zero => new(0);
    public static Money OneCent => new(1);
    public static Money TenCent => new(10);
    public static Money OneEuro => new(100);

    public static Result<Money> FromEuroAndCent(int euro, int euroCent = 0)
    {
        if (euro <= 0)
        {
            return Result<Money>.Fail<InvalidOperationException>($"Invalid ${nameof(euro)} amount");
        }
        if (euroCent is < 0 or > 99)
        {
            return Result<Money>.Fail<InvalidOperationException>($"Invalid {nameof(euroCent)} amount");
        }

        return Result<Money>.Ok(new(CalculateTotalEuroCent(euro, euroCent)));
    }

    public static Result<Money> FromCent(int euroCent)
    {
        if (euroCent < 0)
        {
            return Result<Money>.Fail<InvalidOperationException>($"Invalid {nameof(euroCent)} amount: {euroCent}");
        }

        return Result<Money>.Ok(new(euroCent));
    }

    public static Money operator +(Money left, Money right)
    {
        var moneyResult = FromCent(left.TotalEuroCent + right.TotalEuroCent);
        return moneyResult.ResultValue.EnsureNotNull();
    }

    public static Money operator -(Money left, Money right)
    {
        var moneyResult = FromCent(left.TotalEuroCent - right.TotalEuroCent);
        if (!moneyResult.Succeeded)
        {
            throw moneyResult.Exception.EnsureNotNull();
        }

        return moneyResult.ResultValue.EnsureNotNull();
    }
    
    public static bool operator <(Money left, Money right) => left.TotalEuroCent < right.TotalEuroCent;

    public static bool operator >(Money left, Money right) => left.TotalEuroCent > right.TotalEuroCent;

    public override string ToString()
    {
        var output = (double)TotalEuroCent / (EuroOnly > 0 ? 100 : 10);
        return output.ToString("C2");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TotalEuroCent;
    }

    private static int CalculateTotalEuroCent(int euro, int euroCent) =>
        euro * 100 + euroCent;
}