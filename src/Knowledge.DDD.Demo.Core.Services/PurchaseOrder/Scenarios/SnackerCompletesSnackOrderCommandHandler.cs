using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Infra.Messages;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Scenarios;

public sealed class SnackerCompletesSnackOrderCommandHandler : IRequestHandler<SnackerCompletesSnackOrderCommand, Result<IImmutableList<Snack>>>
{
    private readonly ISnackMachineRepository _snackMachineRepository;
    private readonly ISnackOrderPayment _snackOrderPayment;

    public SnackerCompletesSnackOrderCommandHandler(
        ISnackMachineRepository snackMachineRepository,
        ISnackOrderPayment snackOrderPayment)
    {
        _snackMachineRepository = snackMachineRepository;
        _snackOrderPayment = snackOrderPayment;
    }

    public async Task<Result<IImmutableList<Snack>>> Handle(SnackerCompletesSnackOrderCommand request, CancellationToken cancellationToken)

    {
        var retrieveSnackMachineResult = await _snackMachineRepository.LoadByIdAsync(request.SnackMachineId);

        if (!retrieveSnackMachineResult.Succeeded)
        {
            return Result<IImmutableList<Snack>>.Fail(retrieveSnackMachineResult.Exception.EnsureNotNull());
        }
        
        var snackMachine = retrieveSnackMachineResult.ResultValue.EnsureNotNull();
        var orderCompletedOrder = await snackMachine.SnackerCompletesOrderAsync(request.SnackOrderAmount, _snackOrderPayment);

        if (!orderCompletedOrder.Succeeded)
        {
            return Result<IImmutableList<Snack>>.Fail(orderCompletedOrder.Exception.EnsureNotNull());
        }

        foreach (var domainEvent in snackMachine.DequeueEvents())
        {
            MessageQueue.PublishDomainEvent(domainEvent);
        }

        return orderCompletedOrder;
    }
}