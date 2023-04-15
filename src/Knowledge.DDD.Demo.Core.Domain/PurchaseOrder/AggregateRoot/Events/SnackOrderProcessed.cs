using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot.Events;

public sealed class SnackOrderProcessed: IDomainEvent
{
    public SnackOrderProcessed(
        Money orderAmount,
        IImmutableList<Snack> orderSnacks)
    {
        OrderAmount = orderAmount;
        OrderSnacks = orderSnacks;
    }

    public Money OrderAmount { get; }

    public IImmutableList<Snack> OrderSnacks { get; }
}