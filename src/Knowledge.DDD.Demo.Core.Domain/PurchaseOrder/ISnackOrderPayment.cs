using Knowledge.DDD.Demo.Kernel.Results;

namespace Knowledge.DDD.Demo.Core.Domain.PurchaseOrder;

/// <summary>
/// Domain service which holds to domain logic of making an payment
/// </summary>
public interface ISnackOrderPayment
{
    Task<Result> MakePaymentAsync();
}