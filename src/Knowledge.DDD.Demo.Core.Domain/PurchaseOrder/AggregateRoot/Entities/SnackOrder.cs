using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Events;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Entities;

public sealed class SnackOrder : Entity
{
    private readonly List<(Snack, Money)> _snacksInOrder = new();

    public SnackOrder(Id id) : base(id)
    {
    }

    private IImmutableList<Snack> SnacksInOrder => _snacksInOrder.Select(item => item.Item1).ToImmutableList();

    /// <summary>
    /// Gets the order amount
    /// <remarks>By default the order will be empty</remarks>
    /// </summary>
    public Money OrderAmount { get; private set; } = Money.Zero;

    /// <summary>
    /// Add a new snack to the order
    /// </summary>
    /// <param name="snack"></param>
    /// <param name="addedOrderAmount"></param>
    /// <returns></returns>
    public void AddSnack(
        Snack snack,
        Money addedOrderAmount)
    {
        _snacksInOrder.Add((snack, addedOrderAmount));
        OrderAmount += addedOrderAmount;
    }

    public Result RemoveLastSnack()
    {
        var lastAddedSnack = _snacksInOrder.LastOrDefault();
        var (snack, snackAmount) = lastAddedSnack;
        if (snack is null)
        {
            return Result.Fail<InvalidOperationException>("There is no last snack to remove");
        }

        _snacksInOrder.Remove(lastAddedSnack);
        OrderAmount -= snackAmount;
        return Result.Ok();
    }

    public void ClearOrder()
    {
        _snacksInOrder.Clear();
        OrderAmount = Money.Zero;
    }

    public async Task<Result<(Money orderAmount, IImmutableList<Snack> snacks)>> CompletesOrderAsync(
        Money snackerAddedAmount,
        ISnackOrderPayment snackOrderPayment)
    {
        if (snackerAddedAmount < OrderAmount)
        {
            return Result<(Money, IImmutableList<Snack>)>.Fail<InvalidOperationException>("Amount is less than needed");
        }

        var paymentResult = await snackOrderPayment.MakePaymentAsync();
        if (!paymentResult.Succeeded)
        {
            return Result<(Money, IImmutableList<Snack>)>.Fail(paymentResult.Exception.EnsureNotNull());
        }

        var returnValues = (OrderAmount, SnacksInOrder);
        AddDomainEvent(new SnackOrderProcessed(returnValues.OrderAmount, returnValues.SnacksInOrder));
        ClearOrder();

        return Result<(Money, IImmutableList<Snack>)>.Ok(returnValues);
    }
}