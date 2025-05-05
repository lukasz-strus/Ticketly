using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.Core.Validator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Infrastructure;

[ApiController]
public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator => mediator;

    protected IActionResult BadRequest(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Error",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };

    protected IActionResult NotFound(Result result) => NotFound();

    protected IActionResult NoContent(Unit unit) => NoContent();

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Status = status,
            Detail = error.Message,
            Extensions =
            {
                ["errors"] = errors
            }
        };
}