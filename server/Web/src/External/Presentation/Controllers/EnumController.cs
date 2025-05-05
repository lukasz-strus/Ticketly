using Application.Contracts.Enum;
using Application.Enum.Currency;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Infrastructure;

namespace Presentation.Controllers;

public sealed class EnumController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet(ApiRoutes.Enum.GetCurrencies)]
    [ProducesResponseType(typeof(CurrencyListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrencies() =>
        await Result.Success(new GetCurrenciesQuery())
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);
}