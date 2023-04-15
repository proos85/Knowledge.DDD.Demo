using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Scenarios;

public sealed class SnackerRemovesLastSnackFromOrderCommandHandler : IRequestHandler<SnackerRemovesLastSnackFromOrderCommand, Result>
{
    private readonly ISnackMachineRepository _snackMachineRepository;

    public SnackerRemovesLastSnackFromOrderCommandHandler(ISnackMachineRepository snackMachineRepository)
    {
        _snackMachineRepository = snackMachineRepository;
    }

    public async Task<Result> Handle(SnackerRemovesLastSnackFromOrderCommand request, CancellationToken cancellationToken)
    {
        var retrieveSnackMachineResult = await _snackMachineRepository.LoadByIdAsync(request.SnackMachineId);
        if (!retrieveSnackMachineResult.Succeeded)
        {
            return retrieveSnackMachineResult;
        }

        var snackMachine = retrieveSnackMachineResult.ResultValue.EnsureNotNull();
        var snackerRemoveLastSnackFromOrderResult = snackMachine.SnackerRemoveLastSnackFromOrder();
        if (!snackerRemoveLastSnackFromOrderResult.Succeeded)
        {
            return snackerRemoveLastSnackFromOrderResult;
        }

        return Result.Ok();
    }
}