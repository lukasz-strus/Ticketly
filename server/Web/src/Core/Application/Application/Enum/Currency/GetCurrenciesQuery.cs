using Application.Contracts.Enum;
using Domain.Core.Results;
using MediatR;

namespace Application.Enum.Currency;

public sealed record GetCurrenciesQuery : IRequest<Result<CurrencyListResponse>>;