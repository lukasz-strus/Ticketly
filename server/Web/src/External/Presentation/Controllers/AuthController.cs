using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Register;
using Application.Contracts.Authentication;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Infrastructure;

namespace Presentation.Controllers;

public sealed class AuthController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost(ApiRoutes.Authentication.Register)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new RegisterCommand(value))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);


    [HttpPost(ApiRoutes.Authentication.Login)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new LoginCommand(value))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
}