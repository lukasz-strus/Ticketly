namespace Application.Contracts.Enum;

public sealed record CurrencyListResponse(IReadOnlyCollection<CurrencyResponse> Items);