using System.Diagnostics.CodeAnalysis;
using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.ValueObjects;

namespace Domain.OrderAggregate;

public class OrderItem : Entity<OrderItemId>
{
    public OrderId OrderId { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    [ExcludeFromCodeCoverage] public Order? Order { get; private set; }
    public TicketPoolId TicketPoolId { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    [ExcludeFromCodeCoverage] public TicketPool? TicketPool { get; private set; }
    public uint Quantity { get; private set; }
    public Amount Price { get; private set; }

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    [ExcludeFromCodeCoverage]
    private OrderItem()
    {
        OrderId = null!;
        TicketPoolId = null!;
        Price = null!;
    }

    private OrderItem(
        OrderId orderId,
        TicketPoolId ticketPoolId,
        uint quantity,
        Amount price) : base(new OrderItemId(Guid.NewGuid()))
    {
        OrderId = orderId;
        TicketPoolId = ticketPoolId;
        Quantity = quantity;
        Price = price;
    }

    internal static Result<OrderItem> Create(
        OrderId orderId,
        TicketPoolId ticketPoolId,
        uint quantity,
        Amount price)
    {
        var orderItem = new OrderItem(orderId, ticketPoolId, quantity, price);

        return orderItem;
    }

    internal Result UpdateQuantity(uint quantity)
    {
        if (TicketPool?.AvailableTickets < quantity)
            return Result.Failure(OrderErrors.Update.OrderItem.NotEnoughTicketsAvailable);

        Quantity = quantity;

        return Result.Success();
    }
}