using Application.Contracts.Common;
using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.Create;

public sealed record CreateEventCommand(
    CreateEventRequest Request) : IRequest<Result<EntityCreatedResponse>>;