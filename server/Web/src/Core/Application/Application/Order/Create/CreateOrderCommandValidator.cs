using Application.Core.Utilities;
using Domain;
using Domain.UserAggregate;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Order.Create;

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Request.FirstName)
            .MaximumLength(FirstName.MaxLength).WithError(Errors.ValueObject.FirstNameIsTooLong);

        RuleFor(x => x.Request.LastName)
            .MaximumLength(LastName.MaxLength).WithError(Errors.ValueObject.LastNameIsTooLong);

        RuleFor(x => x.Request.AddressStreet)
            .MaximumLength(Address.StreetMaxLength).WithError(Errors.ValueObject.StreetIsTooLong);

        RuleFor(x => x.Request.AddressBuilding)
            .MaximumLength(Address.BuildingMaxLength).WithError(Errors.ValueObject.BuildingIsTooLong);

        RuleFor(x => x.Request.AddressRoom)
            .MaximumLength(Address.RoomMaxLength).WithError(Errors.ValueObject.RoomIsTooLong);

        When(x => x.Request.AddressCode is not null, () =>
        {
            RuleFor(x => x.Request.AddressCode)
                .MaximumLength(Address.CodeLength).WithError(Errors.ValueObject.CodeIsInvalid);
        });

        RuleFor(x => x.Request.AddressPost)
            .MaximumLength(Address.PostMaxLength).WithError(Errors.ValueObject.PostIsTooLong);
    }
}