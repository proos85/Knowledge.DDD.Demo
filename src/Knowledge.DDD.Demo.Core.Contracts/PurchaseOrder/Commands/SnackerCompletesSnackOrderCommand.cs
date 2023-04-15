using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;

public sealed class SnackerCompletesSnackOrderCommand : IRequest<Result<IImmutableList<Snack>>>
{
    public SnackerCompletesSnackOrderCommand(
        Id snackMachineId,
        Money snackOrderAmount)
    {
        SnackMachineId = snackMachineId;
        SnackOrderAmount = snackOrderAmount;
    }

    public Id SnackMachineId { get; }
    
    public Money SnackOrderAmount { get; }
}