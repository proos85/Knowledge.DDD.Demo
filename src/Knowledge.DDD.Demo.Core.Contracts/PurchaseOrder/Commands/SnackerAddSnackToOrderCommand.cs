using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;

public sealed class SnackerAddSnackToOrderCommand : IRequest<Result>
{
    public SnackerAddSnackToOrderCommand(
        Id snackMachineId,
        Snack snack)
    {
        SnackMachineId = snackMachineId;
        Snack = snack;
    }

    public Id SnackMachineId { get; }

    public Snack Snack { get; }
}