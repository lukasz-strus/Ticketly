using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace Domain.EventAggregate;

public class TicketPool : Entity<TicketPoolId>
{
    public EventId EventId { get; private set; }
    // ReSharper disable once UnusedAutoPropertyAccessor.Local

    [ExcludeFromCodeCoverage] public Event? Event { get; private set; }
    public uint AvailableTickets { get; private set; }
    public Amount Price { get; private set; }
    public DateTime SaleStart { get; private set; }
    public DateTime SaleEnd { get; private set; }

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    [ExcludeFromCodeCoverage]
    private TicketPool()
    {
        EventId = null!;
        Price = null!;
    }

    private TicketPool(
        EventId eventId,
        uint availableTickets,
        Amount price,
        DateTime saleStart,
        DateTime saleEnd) : base(new TicketPoolId(Guid.NewGuid()))
    {
        EventId = eventId;
        AvailableTickets = availableTickets;
        Price = price;
        SaleStart = saleStart;
        SaleEnd = saleEnd;
    }

    internal static Result<TicketPool> Create(
        EventId eventId,
        uint availableTickets,
        Amount price,
        DateTime saleStart,
        DateTime saleEnd)
    {
        if (saleStart > saleEnd)
            return Result.Failure<TicketPool>(
                EventErrors.TicketPools.TicketPoolStartSaleDateMustBeLowerThanEndSaleDate);

        var ticketPool = new TicketPool(eventId, availableTickets, price, saleStart, saleEnd);

        return ticketPool;
    }

    internal Result Update(
        uint availableTickets,
        Amount price,
        DateTime saleStart,
        DateTime saleEnd)
    {
        if (saleStart > saleEnd)
            return Result.Failure<TicketPool>(
                EventErrors.TicketPools.TicketPoolStartSaleDateMustBeLowerThanEndSaleDate);

        AvailableTickets = availableTickets;
        Price = price;
        SaleStart = saleStart;
        SaleEnd = saleEnd;

        return Result.Success();
    }

    public Result RemoveAvailableTickets(uint ticketPoolAvailableTickets)
    {
        if (AvailableTickets < ticketPoolAvailableTickets)
            return Result.Failure(EventErrors.TicketPools.NotEnoughTicketsAvailable);

        AvailableTickets -= ticketPoolAvailableTickets;

        return Result.Success();
    }

    public Result AddAvailableTickets(uint ticketPoolAvailableTickets)
    {
        AvailableTickets += ticketPoolAvailableTickets;

        return Result.Success();
    }
}