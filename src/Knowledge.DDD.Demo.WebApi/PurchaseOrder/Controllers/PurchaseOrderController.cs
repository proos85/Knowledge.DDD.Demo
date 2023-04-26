using System.Collections.Immutable;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Commands;
using Knowledge.DDD.Demo.Core.Contracts.PurchaseOrder.Queries;
using Knowledge.DDD.Demo.Core.Domain.PurchaseOrder.ValueObjects;
using Knowledge.DDD.Demo.Core.Domain.Shared.ValueObjects;
using Knowledge.DDD.Demo.Kernel.Domain;
using Knowledge.DDD.Demo.Kernel.Extensions;
using Knowledge.DDD.Demo.Kernel.Guards;
using Knowledge.DDD.Demo.WebApi.PurchaseOrder.Models;
using Knowledge.DDD.Demo.WebApi.PurchaseOrder.Models.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge.DDD.Demo.WebApi.PurchaseOrder.Controllers;

/// <summary>
/// Controller which handles API actions around purchase order bounded context
/// </summary>
[Route("api/[controller]")]
public class PurchaseOrderController : BaseController
{
    /// <summary>
    /// Create a new instance of <see cref="PurchaseOrderController"/>
    /// </summary>
    /// <param name="mediator"></param>
    public PurchaseOrderController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Initialize a new machine
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(200)]
    [Route("initialize")]
    public async Task<ActionResult<Guid>> DoInitializeSnackMachineAsync()
    {
        var snackMachineId = await Mediator.Send(new InitializeEmptySnackMachineRequest());
        return Ok(snackMachineId.Value);
    }

    /// <summary>
    /// Add snacks to machine
    /// </summary>
    /// <param name="snackMachineIdValue"></param>
    /// <param name="requestBody"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [Route("snackmachine/{snackMachineIdValue:guid:required}/snacks")]
    public async Task<ActionResult> DoAddSnacksInSnackMachineAsync(
        [FromRoute] Guid snackMachineIdValue,
        [FromBody] AddSnacksInSnackMachineBodyData requestBody)
    {
        Check.NotNull(requestBody);

        var (snackMachineId, notFoundException) = RetrieveSnackMachineId(snackMachineIdValue);
        if (notFoundException is not null)
        {
            return ReturnBadRequest(notFoundException);
        }

        var snacksResults = requestBody.Snacks
            .Select(snack => Snack.From(snack.NameOfSnack))
            .ToList();

        if (snacksResults.Any(result => !result.Succeeded))
        {
            return ReturnBadRequest("Not all snacks are valid");
        }

        var snacks = snacksResults.Select(result => result.ResultValue.EnsureNotNull()).ToImmutableList();
        var addSnacksResult = await Mediator.Send(new AddSnacksToMachineCommand(snackMachineId.EnsureNotNull(), snacks));

        return !addSnacksResult.Succeeded 
            ? ReturnBadRequest(addSnacksResult.Exception.EnsureNotNull().Message) 
            : NoContent();
    }
    
    /// <summary>
    /// User add snack to order
    /// </summary>
    /// <param name="snackMachineIdValue">Snack machine identity.</param>
    /// <param name="snackNameValue">Name of the snack.</param>
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [Route("snackmachine/{snackMachineIdValue:guid:required}/order/{snackNameValue:required}")]
    public async Task<ActionResult> DoSnackerAddSnackToOrderAsync(
        [FromRoute] Guid snackMachineIdValue,
        [FromRoute] string snackNameValue)
    {
        Check.NotNullOrWhiteSpace(snackNameValue);

        var (snackMachineId, notFoundException) = RetrieveSnackMachineId(snackMachineIdValue);
        if (notFoundException is not null)
        {
            return ReturnBadRequest(notFoundException);
        }

        var snackResult = Snack.From(snackNameValue);
        if (!snackResult.Succeeded)
        {
            return ReturnBadRequest(snackResult.Exception.EnsureNotNull().Message);
        }

        var addSnacksResult = await Mediator.Send(new SnackerAddSnackToOrderCommand(snackMachineId.EnsureNotNull(), snackResult.ResultValue.EnsureNotNull()));

        return !addSnacksResult.Succeeded 
            ? ReturnBadRequest(addSnacksResult.Exception.EnsureNotNull().Message) 
            : NoContent();
    }
    
    /// <summary>
    /// User remove last from order
    /// </summary>
    /// <param name="snackMachineIdValue">Snack machine identity.</param>
    [HttpDelete]
    [ProducesDefaultResponseType]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [Route("snackmachine/{snackMachineIdValue:guid:required}/order")]
    public async Task<ActionResult> DoSnackerRemovesLastSnackFromOrderAsync([FromRoute] Guid snackMachineIdValue)
    {
        var (snackMachineId, notFoundException) = RetrieveSnackMachineId(snackMachineIdValue);
        if (notFoundException is not null)
        {
            return ReturnBadRequest(notFoundException);
        }
        
        var removeSnackResult = await Mediator.Send(new SnackerRemovesLastSnackFromOrderCommand(snackMachineId.EnsureNotNull()));

        return !removeSnackResult.Succeeded 
            ? ReturnBadRequest(removeSnackResult.Exception.EnsureNotNull().Message) 
            : NoContent();
    }

    /// <summary>
    /// User clear snack order
    /// </summary>
    /// <param name="snackMachineIdValue">Snack machine identity.</param>
    [HttpDelete]
    [ProducesDefaultResponseType]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [Route("snackmachine/{snackMachineIdValue:guid:required}/order/clear")]
    public async Task<ActionResult> DoSnackerClearSnackOrderAsync([FromRoute] Guid snackMachineIdValue)
    {
        var (snackMachineId, notFoundException) = RetrieveSnackMachineId(snackMachineIdValue);
        if (notFoundException is not null)
        {
            return ReturnBadRequest(notFoundException);
        }

        var clearOrderResult = await Mediator.Send(new SnackerClearSnackOrderCommand(snackMachineId.EnsureNotNull()));

        return !clearOrderResult.Succeeded
            ? ReturnBadRequest(clearOrderResult.Exception.EnsureNotNull().Message)
            : NoContent();
    }

    /// <summary>
    /// User completes order
    /// </summary>
    /// <param name="snackMachineIdValue">Snack machine identity.</param>
    /// <param name="requestBodyData">Request body data</param>
    [HttpPost]
    [ProducesDefaultResponseType]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Route("snackmachine/{snackMachineIdValue:guid:required}/order/finalize")]
    public async Task<ActionResult<IReadOnlyList<SnackDto>>> DoSnackerCompletesSnackOrder(
        [FromRoute] Guid snackMachineIdValue,
        [FromBody] UserCompletesSnackOrderBodyData requestBodyData)
    {
        Check.NotNull(requestBodyData);
        
        var (snackMachineId, notFoundException) = RetrieveSnackMachineId(snackMachineIdValue);
        if (notFoundException is not null)
        {
            return ReturnBadRequest(notFoundException);
        }

        var snackerAddedAmount = Money.FromCent(requestBodyData.SnackOrderAmountCents);
        if (!snackerAddedAmount.Succeeded)
        {
            return ReturnBadRequest($"Invalid added amount: {requestBodyData.SnackOrderAmountCents}");
        }

        var completedOrderResult = await Mediator.Send(new SnackerCompletesSnackOrderCommand(
            snackMachineId.EnsureNotNull(),
            snackerAddedAmount.ResultValue.EnsureNotNull()));

        return !completedOrderResult.Succeeded 
            ? ReturnBadRequest(completedOrderResult.Exception.EnsureNotNull()) 
            : Ok(completedOrderResult.ResultValue);
    }

    private (Id? id, Exception? exceptionMessage) RetrieveSnackMachineId(Guid snackMachineIdValue)
    {
        var snackMachineIdResult = Id.From(snackMachineIdValue);
        if (!snackMachineIdResult.Succeeded)
        {
            return (null, snackMachineIdResult.Exception.EnsureNotNull());
        }

        return (snackMachineIdResult.ResultValue.EnsureNotNull(), null);
    }
}