using Domain.Core.Results;
using Domain.Enums;

namespace Domain.ValueObjects;

public sealed record Amount
{
    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    private Amount()
    {
        Value = 0;
        Currency = Currency.None;
    }

    private Amount(decimal value, Currency currency)
    {
        Value = value;
        Currency = currency;
    }

    public decimal Value { get; init; }
    public Currency Currency { get; init; }

    public static Result<Amount> Create(decimal price, Currency currency)
    {
        if (price <= 0)
            return Result.Failure<Amount>(Errors.ValueObject.AmountValueMustBeGreaterThan0);

        return new Amount(price, currency);
    }
}