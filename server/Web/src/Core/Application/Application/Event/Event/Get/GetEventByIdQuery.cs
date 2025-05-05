using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.Get;

public sealed record GetEventByIdQuery(
    Guid Id) : IRequest<Result<EventResponse>>;