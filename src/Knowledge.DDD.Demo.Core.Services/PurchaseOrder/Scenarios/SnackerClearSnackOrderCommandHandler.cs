using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Scenarios;

public sealed class SnackerClearSnackOrderCommandHandler : IRequestHandler<SnackerClearSnackOrderCommand, Result>
{
    private readonly ISnackMachineRepository _snackMachineRepository;

    public SnackerClearSnackOrderCommandHandler(ISnackMachineRepository snackMachineRepository)
    {
        _snackMachineRepository = snackMachineRepository;
    }

    public async Task<Result> Handle(SnackerClearSnackOrderCommand request, CancellationToken cancellationToken)
    {
        var retrieveSnackMachineResult = await _snackMachineRepository.LoadByIdAsync(request.SnackMachineId);
        if (!retrieveSnackMachineResult.Succeeded)
        {
            return retrieveSnackMachineResult;
        }

        var snackMachine = retrieveSnackMachineResult.ResultValue.EnsureNotNull();
        snackMachine.SnackerClearsOrder();
        
        return Result.Ok();
    }
}