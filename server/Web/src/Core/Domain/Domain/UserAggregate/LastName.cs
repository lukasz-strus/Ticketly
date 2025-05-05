using Domain.Core.Results;

namespace Domain.UserAggregate;

public sealed record LastName
{
    private LastName(string value) => Value = value;

    public const int MaxLength = 50;

    public string Value { get; init; }

    public static Result<LastName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<LastName>(Errors.ValueObject.LastNameIsRequired);

        if (value.Length > MaxLength)
            return Result.Failure<LastName>(Errors.ValueObject.LastNameIsTooLong);

        return new LastName(value);
    }
}