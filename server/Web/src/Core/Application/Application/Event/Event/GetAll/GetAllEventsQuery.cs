using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.GetAll;

public sealed record GetAllEventsQuery(Guid? CategoryId) : IRequest<Result<EventListResponse>>;