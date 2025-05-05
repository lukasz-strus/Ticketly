using Domain.Core.Results;

namespace Domain.ValueObjects;

public sealed record Name
{
    private Name(string value) => Value = value;

    public const int MaxLength = 100;

    public string Value { get; init; }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Name>(Errors.ValueObject.NameIsRequired);

        if (value.Length > MaxLength)
            return Result.Failure<Name>(Errors.ValueObject.NameIsTooLong);

        return new Name(value);
    }
}