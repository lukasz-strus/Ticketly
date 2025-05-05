using Application.Contracts.Event;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Event.Get;

internal sealed class GetEventByIdQueryHandler(
    IEventRepository eventRepository) : IRequestHandler<GetEventByIdQuery, Result<EventResponse>>
{
    public async Task<Result<EventResponse>> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.Id), cancellationToken);
        if (@event is null)
            return Result.Failure<EventResponse>(Errors.General.EntityNotFound);

        return Result.Success(new EventResponse(
            @event.Id.Value,
            @event.Name.Value,
            @event.CategoryId.Value,
            @event.Category?.Name.Value,
            @event.Description.Value,
            @event.Location.Street,
            @event.Location.Building,
            @event.Location.Room,
            @event.Location.Code,
            @event.Location.Post,
            @event.Date,
            @event.ImageUrl,
            new TicketPoolListResponse(
            [
                ..@event.TicketPools.Select(tp => new TicketPoolResponse(
                    tp.Id.Value,
                    tp.AvailableTickets,
                    tp.Price.Value,
                    tp.Price.Currency.Value,
                    Currency.FromValue(tp.Price.Currency.Value)!.Format(tp.Price.Value),
                    tp.SaleStart,
                    tp.SaleEnd
                ))
            ])));
    }
}