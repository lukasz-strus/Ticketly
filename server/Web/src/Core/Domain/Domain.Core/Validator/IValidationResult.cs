using Domain.Core.Primitives;

namespace Domain.Core.Validator;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "A validation problem occurred.");

    Error[] Errors { get; }
}