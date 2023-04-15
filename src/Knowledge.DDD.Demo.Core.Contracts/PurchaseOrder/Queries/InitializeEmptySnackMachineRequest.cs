using Knowledge.DDD.Demo.Kernel.Domain;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Queries;

public sealed class InitializeEmptySnackMachineRequest : IRequest<Id>
{

}