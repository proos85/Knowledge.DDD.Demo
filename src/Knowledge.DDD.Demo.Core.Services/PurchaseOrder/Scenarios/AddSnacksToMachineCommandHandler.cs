using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Scenarios;

public sealed class AddSnacksToMachineCommandHandler : IRequestHandler<AddSnacksToMachineCommand, Result>
{
    private readonly ISnackMachineRepository _snackMachineRepository;

    public AddSnacksToMachineCommandHandler(ISnackMachineRepository snackMachineRepository)
    {
        _snackMachineRepository = snackMachineRepository;
    }

    public async Task<Result> Handle(AddSnacksToMachineCommand request, CancellationToken cancellationToken)
    {
        var retrieveSnackMachineResult = await _snackMachineRepository.LoadByIdAsync(request.SnackMachineId);
        if (!retrieveSnackMachineResult.Succeeded)
        {
            return retrieveSnackMachineResult;
        }

        var snackMachine = retrieveSnackMachineResult.ResultValue.EnsureNotNull();
        snackMachine.AddSnacksToSlot(request.Snacks);
        return Result.Ok();
    }
}