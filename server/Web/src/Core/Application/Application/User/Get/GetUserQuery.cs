using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.User.Get;

public sealed record GetUserQuery : IRequest<Result<UserResponse>>;