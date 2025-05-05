using Application.Contracts.Authentication;
using Domain.Core.Results;
using MediatR;

namespace Application.Authentication.Commands.Login;

public sealed record LoginCommand(
    LoginRequest Request) : IRequest<Result<TokenResponse>>;
