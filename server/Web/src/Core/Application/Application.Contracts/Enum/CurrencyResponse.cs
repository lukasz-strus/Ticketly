namespace Application.Contracts.Enum;

public sealed record CurrencyResponse(
    int Id,
    string Name,
    string Code);