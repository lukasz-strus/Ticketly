using Domain.Core.Results;

namespace Domain.UserAggregate;

public sealed record FirstName
{
    private FirstName(string value) => Value = value;

    public const int MaxLength = 50;

    public string Value { get; init; }

    public static Result<FirstName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<FirstName>(Errors.ValueObject.FirstNameIsRequired);

        if (value.Length > MaxLength)
            return Result.Failure<FirstName>(Errors.ValueObject.FirstNameIsTooLong);

        return new FirstName(value);
    }
}