using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;

public sealed class SnackerRemovesLastSnackFromOrderCommand : IRequest<Result>
{
    public SnackerRemovesLastSnackFromOrderCommand(Id snackMachineId)
    {
        SnackMachineId = snackMachineId;
    }

    public Id SnackMachineId { get; }
}