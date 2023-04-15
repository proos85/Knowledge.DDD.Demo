namespace Knowledge.DDD.Demo.WebApi.PurchaseOrder.Models.Requests;

public sealed class AddSnacksInSnackMachineBodyData
{
    public IReadOnlyList<SnackDto> Snacks { get; init; }
}