using Application.Contracts.Common;
using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.AddTicketPool;

public sealed record AddTicketPoolCommand(
    Guid EventId,
    AddTicketPoolRequest Request) : IRequest<Result<EntityCreatedResponse>>;