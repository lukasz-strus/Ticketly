using Application.Contracts.Event;
using Domain.Core.Results;
using Domain.Enums;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Event.GetAll;

internal sealed class GetAllEventsQueryHandler(
    IEventRepository eventRepository) : IRequestHandler<GetAllEventsQuery, Result<EventListResponse>>
{
    public async Task<Result<EventListResponse>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = request.CategoryId is null
            ? await eventRepository.GetAllEventsAsync(cancellationToken)
            : await eventRepository.GetAllEventsAsync(new CategoryId(request.CategoryId.Value), cancellationToken);

        return Result.Success(new EventListResponse(
        [
            ..events.Select(@event => new EventResponse(
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
                ])))
        ]));
    }
}