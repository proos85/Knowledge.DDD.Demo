using Knowledge.DDD.Demo.WebApi.PurchaseOrder.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Knowledge.DDD.Demo.WebApi.PurchaseOrder.Controllers;

[ApiController]
[AllowAnonymous]
public class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;

    public BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected ActionResult ReturnBadRequest(Exception exception) =>
        ReturnBadRequest(exception.Message);

    protected ActionResult ReturnBadRequest(string message) =>
        BadRequest(new BadRequestResponse
        {
            Message = message
        });
}