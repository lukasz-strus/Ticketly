using Domain.Core.Results;

namespace Domain.ValueObjects;

public sealed record Description
{
    private Description(string value) => Value = value;

    public string Value { get; init; }

    public static Result<Description> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Description>(Errors.ValueObject.DescriptionIsRequired);

        return new Description(value);
    }

}