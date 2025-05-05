using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.Update;

public sealed record UpdateEventCommand(
    Guid Id,
    UpdateEventRequest Request) : IRequest<Result>;
