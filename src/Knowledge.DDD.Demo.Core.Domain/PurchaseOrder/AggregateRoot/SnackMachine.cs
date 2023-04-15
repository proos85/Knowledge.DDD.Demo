using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Entities;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot;

public sealed class SnackMachine: Kernel.Domain.AggregateRoot
{
    private readonly List<SnackSlot> _slots = new();
    private readonly SnackOrder _snackOrder;
    
    private SnackMachine(Id id) : base(id)
    {
        _snackOrder = new SnackOrder(Id.New());
    }

    public ImmutableList<SnackSlot> Slots => _slots.ToImmutableList();

    public SnackOrder SnackOrder => _snackOrder;

    public Money AmountOfMoneyInSnackMachine { get; private set; } = Money.Zero;

    public static SnackMachine Empty()
    {
        var snackMachine = new SnackMachine(Id.New());
        snackMachine.AddSlots();

        return snackMachine;
    }

    public Result AddSnacksToSlot(IReadOnlyList<Snack> snacks)
    {
        foreach (var snack in snacks)
        {
            AddSnackToSlot(snack);
        }
        return Result.Ok();
    }

    public Result SnackerAddSnackToOrder(Snack snack)
    {
        var slotResult = GetSlotForMatchSnack(snack);
        if (!slotResult.Succeeded)
        {
            return Result.Fail<InvalidOperationException>("Unable to retrieve snack");
        }

        var slot = slotResult.ResultValue.EnsureNotNull();
        var anySnacksInSlotResult = slot.AreThereAnySnacksInSlot();
        if (!anySnacksInSlotResult.Succeeded)
        {
            return anySnacksInSlotResult;
        }

        _snackOrder.AddSnack(snack, slot.PricePerSnackInSlot);
        return Result.Ok();
    }
    
    public Result SnackerRemoveLastSnackFromOrder() => _snackOrder.RemoveLastSnack();

    public void SnackerClearsOrder() => _snackOrder.ClearOrder();

    public async Task<Result<IImmutableList<Snack>>> SnackerCompletesOrderAsync(
        Money addedAmount,
        ISnackOrderPayment snackOrderPayment)
    {
        var completeOrderResult = await _snackOrder.CompletesOrderAsync(addedAmount, snackOrderPayment);
        if (!completeOrderResult.Succeeded)
        {
            return Result<IImmutableList<Snack>>.Fail(completeOrderResult.Exception.EnsureNotNull());
        }

        var (orderAmount, snacksFromOrder) = completeOrderResult.ResultValue.EnsureNotNull();
        foreach (var snack in snacksFromOrder)
        {
            var slot = GetSlotForMatchSnack(snack).ResultValue.EnsureNotNull();
            slot.RemoveSnack(snack);
        }

        AmountOfMoneyInSnackMachine += orderAmount;

        foreach (var domainEvent in _snackOrder.DequeueEvents())
        {
            AddDomainEvent(domainEvent);
        }
        
        return Result<IImmutableList<Snack>>.Ok(snacksFromOrder);
    }

    private void AddSlots()
    {
        // Based on the allowed snacks, each slot will have it's own snack (mars on one slot, snickers on another, etc)
        _slots.AddRange(SnackName.AllowedSnackNames
            .Select(snackName => new SnackSlot(
                Id.New(),
                snackName,
                Money.FromEuroAndCent(3).ResultValue.EnsureNotNull())));
    }

    private Result AddSnackToSlot(Snack snack)
    {
        var slotResult = GetSlotForMatchSnack(snack);
        if (!slotResult.Succeeded)
        {
            return slotResult;
        }

        var slot = slotResult.ResultValue.EnsureNotNull();
        slot.AddSnack(snack);
        return Result.Ok();
    }

    private Result<SnackSlot> GetSlotForMatchSnack(Snack snack)
    {
        var slot = _slots.SingleOrDefault(slot => slot.NameOfSnackInSlot == snack.SnackName);
        if (slot is null)
        {
            return Result<SnackSlot>.Fail<InvalidOperationException>("No slot found for snack");
        }

        return Result<SnackSlot>.Ok(slot);
    }
}