using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.RemoveTicketPool;

public sealed record RemoveTicketPoolCommand(
    Guid EventId,
    Guid TicketPoolId) : IRequest<Result>;
