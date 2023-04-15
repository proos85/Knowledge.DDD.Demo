using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.AggregateRoot;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Repositories;

public interface ISnackMachineRepository
{
    Task SaveAsync(SnackMachine snackMachine);

    Task<Result<SnackMachine>> LoadByIdAsync(Id id);
}