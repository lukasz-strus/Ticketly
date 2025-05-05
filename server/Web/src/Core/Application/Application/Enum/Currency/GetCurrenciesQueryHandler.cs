using Application.Contracts.Enum;
using Domain.Core.Results;
using MediatR;

namespace Application.Enum.Currency;

internal sealed class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, Result<CurrencyListResponse>>
{
    public Task<Result<CurrencyListResponse>> Handle(
        GetCurrenciesQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.Enums.Currency> currencies =
        [
            Domain.Enums.Currency.Usd,
            Domain.Enums.Currency.Eur,
            Domain.Enums.Currency.Pln
        ];

        return Task.FromResult(Result.Success(new CurrencyListResponse(
            [
                ..currencies.Select(x => new CurrencyResponse(
                    x.Value,
                    x.Name,
                    x.Code))
            ]
        )));
    }
}