using Domain;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Event.Category.Create;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage(Errors.ValueObject.NameIsRequired)
            .MaximumLength(Name.MaxLength).WithMessage(Errors.ValueObject.NameIsTooLong);
    }
}
