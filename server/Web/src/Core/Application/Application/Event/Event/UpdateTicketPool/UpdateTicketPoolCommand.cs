using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.UpdateTicketPool;

public sealed record UpdateTicketPoolCommand(
    Guid EventId,
    Guid TicketPoolId,
    UpdateTicketPoolRequest Request) : IRequest<Result>;
