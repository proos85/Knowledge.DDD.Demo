using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Queries;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot;
using Knowledge.DDD.Demo.Kernel.Domain;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Scenarios;

public sealed class InitializeEmptySnackMachineRequestHandler: IRequestHandler<InitializeEmptySnackMachineRequest, Id>
{
    private readonly ISnackMachineRepository _snackMachineRepository;

    public InitializeEmptySnackMachineRequestHandler(ISnackMachineRepository snackMachineRepository)
    {
        _snackMachineRepository = snackMachineRepository;
    }

    public async Task<Id> Handle(InitializeEmptySnackMachineRequest request, CancellationToken cancellationToken)
    {
        var snackMachine = SnackMachine.Empty();
        await _snackMachineRepository.SaveAsync(snackMachine);

        return snackMachine.Id;
    }
}