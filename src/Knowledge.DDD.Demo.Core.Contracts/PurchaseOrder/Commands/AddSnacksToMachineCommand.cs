using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;

public sealed class AddSnacksToMachineCommand : IRequest<Result>
{
    public AddSnacksToMachineCommand(
        Id snackMachineId,
        IImmutableList<Snack> snacks)
    {
        SnackMachineId = snackMachineId;
        Snacks = snacks;
    }

    public Id SnackMachineId { get; }

    public IImmutableList<Snack> Snacks { get; }
}