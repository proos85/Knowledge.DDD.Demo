using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;

public sealed class SnackName: ValueObject
{
    public static readonly IReadOnlyList<SnackName> AllowedSnackNames =
        Enum.GetValues<AllowedSnackName>()
            .Select(allowedSnackName => From(allowedSnackName).ResultValue.EnsureNotNull())
            .ToList();

    private readonly AllowedSnackName _allowedSnackName;
    
    private enum AllowedSnackName
    {
        // ReSharper disable UnusedMember.Local
        RegularMars,
        LargeMars,
        RegularSnickers,
        DoubleSnickers
        // ReSharper restore UnusedMember.Local
    }

    private SnackName(AllowedSnackName name)
    {
        _allowedSnackName = name;
    }

    public string Name => _allowedSnackName.ToString();

    public static SnackName RegularMars => new(AllowedSnackName.RegularMars);
    public static SnackName LargeMars => new(AllowedSnackName.LargeMars);
    public static SnackName RegularSnickers => new(AllowedSnackName.RegularSnickers);
    public static SnackName DoubleSnickers => new(AllowedSnackName.DoubleSnickers);

    public static Result<SnackName> From(string snackName)
    {
        if (string.IsNullOrWhiteSpace(snackName))
        {
            return Result<SnackName>.Fail<ArgumentException>("No snack name");
        }

        if (!Enum.TryParse(snackName, true, out AllowedSnackName allowedSnackName))
        {
            return Result<SnackName>.Fail<InvalidOperationException>("Not a valid snack name");
        }

        return From(allowedSnackName);
    }

    public override string ToString() => Name;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }

    private static Result<SnackName> From(AllowedSnackName snackName) => 
        Result<SnackName>.Ok(new(snackName));
}