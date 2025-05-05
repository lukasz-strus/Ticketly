using Application.Core.Utilities;
using Domain;
using FluentValidation;

namespace Application.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email).NotEmpty().WithError(Errors.Authentication.MailIsRequired);

        RuleFor(x => x.Request.Password).NotEmpty().WithError(Errors.Authentication.PasswordIsRequired);
    }
}
