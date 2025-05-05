using Application.Contracts.Authentication;
using Domain.Core.Results;
using MediatR;

namespace Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    RegisterRequest Request) : IRequest<Result>;
