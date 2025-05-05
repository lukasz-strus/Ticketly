using Domain;
using FluentValidation;

namespace Application.Event.Event.Delete;

internal sealed class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Errors.ValueObject.IdIsRequired);
    }
}
