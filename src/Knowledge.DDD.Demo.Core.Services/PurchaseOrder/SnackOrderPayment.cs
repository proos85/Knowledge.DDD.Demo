using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder;
using Knowledge.DDD.Demo.Kernel.Results;
using MediatR;

namespace Knowledge.DDD.Demo.Core.Services.PurchaseOrder;

/// <summary>
/// This is a domain service used for domain business order when performing a payment transaction
/// </summary>
internal sealed class SnackOrderPayment: ISnackOrderPayment
{
    private readonly IMediator _mediator;

    public SnackOrderPayment(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Performs the payment transaction
    /// </summary>
    /// <returns></returns>
    async Task<Result> ISnackOrderPayment.MakePaymentAsync()
    {
        // This does not really makes sense to place this in a domain service, because only a command is executed
        // an no business logic so-ever. This is just an example
        var result = await _mediator.Send(new PurchaseOrderPaymentTransactionCommand());
        return !result 
            ? Result.Fail<InvalidOperationException>("Unable tot complete payment") 
            : Result.Ok();
    }
}