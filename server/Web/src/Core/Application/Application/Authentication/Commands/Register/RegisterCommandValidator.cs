using Application.Core.Utilities;
using Domain;
using Domain.UserAggregate;
using Domain.ValueObjects;
using FluentValidation;
using Application.Core.Regex;

namespace Application.Authentication.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().WithError(Errors.Authentication.MailIsRequired)
            .EmailAddress().WithError(Errors.ValueObject.WrongEmailFormat);

        RuleFor(x => x.Request.Password)
            .NotEmpty().WithError(Errors.Authentication.PasswordIsRequired)
            .Must(password => PasswordRegex.PasswordPattern().IsMatch(password)).WithError(Errors.Authentication.PasswordIsNotStrongEnough);
        
        RuleFor(x => x.Request.FirstName)
            .NotEmpty().WithError(Errors.ValueObject.FirstNameIsRequired)
            .MaximumLength(FirstName.MaxLength).WithError(Errors.ValueObject.FirstNameIsTooLong);
        
        RuleFor(x => x.Request.LastName)
            .NotEmpty().WithError(Errors.ValueObject.LastNameIsRequired)
            .MaximumLength(LastName.MaxLength).WithError(Errors.ValueObject.LastNameIsTooLong);
        
        RuleFor(x => x.Request.Street)
            .NotEmpty().WithError(Errors.ValueObject.StreetIsRequired)
            .MaximumLength(Address.StreetMaxLength).WithError(Errors.ValueObject.StreetIsTooLong);
        
        RuleFor(x => x.Request.Building)
            .NotEmpty().WithError(Errors.ValueObject.BuildingIsRequired)
            .MaximumLength(Address.BuildingMaxLength).WithError(Errors.ValueObject.BuildingIsTooLong);
        
        RuleFor(x => x.Request.Room)
            .MaximumLength(Address.RoomMaxLength).WithError(Errors.ValueObject.RoomIsTooLong);

        RuleFor(x => x.Request.Code)
            .NotEmpty().WithError(Errors.ValueObject.CodeIsRequired)
            .Length(Address.CodeLength).WithError(Errors.ValueObject.CodeMustBe5CharactersLong);
        
        RuleFor(x => x.Request.Post)
            .NotEmpty().WithError(Errors.ValueObject.PostIsRequired)
            .MaximumLength(Address.PostMaxLength).WithError(Errors.ValueObject.PostIsTooLong);
    }
}


