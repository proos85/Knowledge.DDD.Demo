using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Entities;

public sealed class SnackSlot: Entity
{
    private const int MaxAmountOfSnacksInSlot = 50;

    private readonly List<Snack> _snacks = new();

    public SnackSlot(
        Id id,
        SnackName nameOfSnackInSlot,
        Money pricePerSnackInSlot) : base(id)
    {
        NameOfSnackInSlot = nameOfSnackInSlot;
        PricePerSnackInSlot = pricePerSnackInSlot;
    }

    public SnackName NameOfSnackInSlot { get; }
    
    public Money PricePerSnackInSlot { get; }

    public int AmountOfSnacksInSlot => _snacks.Count;

    public ImmutableList<Snack> Snacks => _snacks.ToImmutableList();

    public Result AddSnack(Snack snack)
    {
        var validateIfSnackBelongsToSlotResult = ValidateIfSnackBelongsToSlot(snack);
        if (!validateIfSnackBelongsToSlotResult.Succeeded)
        {
            return validateIfSnackBelongsToSlotResult;
        }

        if (AmountOfSnacksInSlot + 1 > MaxAmountOfSnacksInSlot)
        {
            return Result.Fail<InvalidOperationException>("Slot is full");
        }

        _snacks.Add(snack);
        return Result.Ok();
    }

    public Result RemoveSnack(Snack snack)
    {
        var validateIfSnackBelongsToSlotResult = ValidateIfSnackBelongsToSlot(snack);
        if (!validateIfSnackBelongsToSlotResult.Succeeded)
        {
            return validateIfSnackBelongsToSlotResult;
        }

        if (AmountOfSnacksInSlot -1 < 0)
        {
            return Result.Fail<InvalidOperationException>("Slot is empty");
        }

        _snacks.Remove(snack);
        return Result.Ok();
    }

    public Result AreThereAnySnacksInSlot() => 
        _snacks.Any() ? Result.Ok() : Result.Fail<InvalidOperationException>("No more snacks in slot");

    private Result ValidateIfSnackBelongsToSlot(Snack snack) =>
        snack.SnackName != NameOfSnackInSlot 
            ? Result.Fail<InvalidOperationException>("Snack does not belong in this slot") 
            : Result.Ok();
}