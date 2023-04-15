using System.Collections.Concurrent;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder.Repositories;

internal sealed class SnackMachineRepository: ISnackMachineRepository
{
    private static readonly ConcurrentDictionary<Id, SnackMachine> InMemory = new();

    public Task SaveAsync(SnackMachine snackMachine)
    {
        InMemory.TryAdd(snackMachine.Id, snackMachine);
        return Task.CompletedTask;
    }

    public Task<Result<SnackMachine>> LoadByIdAsync(Id id)
    {
        return Task.FromResult(!InMemory.TryGetValue(id, out SnackMachine? snackMachine) 
            ? Result<SnackMachine>.Fail<InvalidOperationException>($"Unable to load snack machine by id: {id.Value}") 
            : Result<SnackMachine>.Ok(snackMachine));
    }
}