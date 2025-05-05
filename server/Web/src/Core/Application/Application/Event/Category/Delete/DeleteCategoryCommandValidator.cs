using Domain;
using FluentValidation;

namespace Application.Event.Category.Delete;

internal sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Errors.ValueObject.IdIsRequired);
    }
}
