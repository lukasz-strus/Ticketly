using Application.Order.Get;
using Application.User.Get;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Infrastructure;

namespace Presentation.Controllers;

public sealed class UserController(IMediator mediator) : ApiController(mediator)
{
    [Authorize]
    [HttpGet(ApiRoutes.User.Me)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Me() =>
        await Mediator.Send(new GetUserQuery())
            .Match(Ok, BadRequest);


    [Authorize]
    [HttpGet(ApiRoutes.User.GetOrders)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrders() =>
        await Mediator.Send(new GetUserOrdersQuery())
            .Match(Ok, BadRequest);
}