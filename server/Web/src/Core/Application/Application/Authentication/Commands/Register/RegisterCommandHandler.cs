using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.UserAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository) : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
        await CreateUserResult(
                request.Request.Email.Split('@')[0],
                request.Request.Email,
                FirstName.Create(request.Request.FirstName),
                LastName.Create(request.Request.LastName),
                Address.Create(
                    request.Request.Street,
                    request.Request.Building,
                    request.Request.Room,
                    request.Request.Code,
                    request.Request.Post))
            .Ensure(async user => await userRepository.IsEmailUniqueAsync(user.Email),
                Errors.Authentication.DuplicateEmail)
            .Tap(async user => await userRepository.Insert(user, request.Request.Password));

    private static Result<Domain.UserAggregate.User> CreateUserResult(
        string userName,
        string email,
        Result<FirstName> firstNameResult,
        Result<LastName> lastNameResult,
        Result<Address> addressResult) =>
        Result
            .FirstFailureOrSuccess(firstNameResult, lastNameResult, addressResult)
            .Map(() => Result.Success(
                new Domain.UserAggregate.User()
                {
                    UserName = userName,
                    Email = email,
                    FirstName = firstNameResult.Value(),
                    LastName = lastNameResult.Value(),
                    Address = addressResult.Value()
                }));
}