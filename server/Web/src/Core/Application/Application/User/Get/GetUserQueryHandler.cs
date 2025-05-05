using Application.Authentication.UserContext;
using Application.Contracts.User;
using Domain;
using Domain.Core.Results;
using Domain.UserAggregate;
using MediatR;

namespace Application.User.Get;

internal sealed class GetUserQueryHandler(
    IUserRepository userRepository,
    IUserContext userContext) : IRequestHandler<GetUserQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
            return Result.Failure<UserResponse>(Errors.General.BadRequest);

        var user = await userRepository.GetByIdAsync(currentUser.Id, cancellationToken);
        if (user == null)
            return Result.Failure<UserResponse>(Errors.General.EntityNotFound);

        var role = await userRepository.GetUserRole(user.Id, cancellationToken);
        if (!role.Equals(UserRole.Admin) && !role.Equals(UserRole.User))
            return Result.Failure<UserResponse>(Errors.General.BadRequest);

        return Result.Success(new UserResponse(
            user.Id,
            user.Email!,
            user.FirstName.Value,
            user.LastName.Value,
            user.Address.Street,
            user.Address.Building,
            user.Address.Room,
            user.Address.Code,
            user.Address.Post,
            role.Name,
            role.Value));
    }
}