using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using MediatR;

namespace Knowledge.DDD.Demo.Infra.Payment.PurchaseOrder.Handlers;

public sealed class PurchaseOrderPaymentTransactionCommandHandler: IRequestHandler<PurchaseOrderPaymentTransactionCommand, bool>
{
    public Task<bool> Handle(PurchaseOrderPaymentTransactionCommand request, CancellationToken cancellationToken)
    {
        return Observable
            .FromAsync(() => DoCallAsync(cancellationToken))
            .Select(_ => true)
            .Retry(2)
            .Catch(Observable.Return(false))
            .ToTask(cancellationToken);
    }

    private Task DoCallAsync(CancellationToken cancellationToken) => 
        Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
}