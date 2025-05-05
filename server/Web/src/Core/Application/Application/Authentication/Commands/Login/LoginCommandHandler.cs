using Application.Contracts.Authentication;
using Application.Core.Abstractions.Authentication;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.UserAggregate.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    UserManager<Domain.UserAggregate.User> userManager,
    IPasswordChecker passwordChecker,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<TokenResponse>>
{
    public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
        await Result.Success(request)
            .Bind(
                command => userManager.FindByEmailAsync(command.Request.Email),
                Errors.Authentication.InvalidEmailOrPassword)
            .Ensure(
                user => user.VerifyPassword(request.Request.Password, passwordChecker),
                Errors.Authentication.InvalidEmailOrPassword)
            .Bind(jwtProvider.CreateAsync);
}