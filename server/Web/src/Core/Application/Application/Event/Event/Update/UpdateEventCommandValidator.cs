using Domain;
using FluentValidation;

namespace Application.Event.Event.Update;

internal sealed class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage(Errors.ValueObject.NameIsRequired)
            .MaximumLength(100).WithMessage(Errors.ValueObject.NameIsTooLong);

        RuleFor(x => x.Request.CategoryId)
            .NotEmpty().WithMessage(Errors.ValueObject.CategoryIdIsRequired);

        RuleFor(x => x.Request.Description)
            .NotEmpty().WithMessage(Errors.ValueObject.DescriptionIsRequired);

        RuleFor(x => x.Request.LocationStreet)
            .NotEmpty().WithMessage(Errors.ValueObject.StreetIsRequired)
            .MaximumLength(100).WithMessage(Errors.ValueObject.StreetIsTooLong);

        RuleFor(x => x.Request.LocationBuilding)
            .NotEmpty().WithMessage(Errors.ValueObject.BuildingIsRequired)
            .MaximumLength(100).WithMessage(Errors.ValueObject.BuildingIsTooLong);

        RuleFor(x => x.Request.LocationRoom)
            .MaximumLength(100).WithMessage(Errors.ValueObject.RoomIsTooLong);

        RuleFor(x => x.Request.LocationCode)
            .NotEmpty().WithMessage(Errors.ValueObject.CodeIsRequired)
            .Length(5).WithMessage(Errors.ValueObject.CodeMustBe5CharactersLong);

        RuleFor(x => x.Request.LocationPost)
            .NotEmpty().WithMessage(Errors.ValueObject.PostIsRequired)
            .MaximumLength(100).WithMessage(Errors.ValueObject.PostIsTooLong);

        RuleFor(x => x.Request.Date)
            .NotEmpty().WithMessage(Errors.ValueObject.DateIsRequired);

    }
}
