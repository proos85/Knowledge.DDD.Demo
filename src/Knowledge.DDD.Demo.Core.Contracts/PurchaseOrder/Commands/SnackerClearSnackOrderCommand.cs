using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;

public sealed class SnackerClearSnackOrderCommand : IRequest<Result>
{
    public SnackerClearSnackOrderCommand(Id snackMachineId)
    {
        SnackMachineId = snackMachineId;
    }

    public Id SnackMachineId { get; }
}