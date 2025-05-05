using System.Diagnostics.CodeAnalysis;
using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.UserAggregate;
using Domain.ValueObjects;

namespace Domain.OrderAggregate;

public class Order : AggregateRoot<OrderId>
{
    private readonly HashSet<OrderItem> _orderItems = [];
    public string? UserId { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    [ExcludeFromCodeCoverage] public User? User { get; private set; }
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Address Address { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    [ExcludeFromCodeCoverage]
    private Order()
    {
        FirstName = null!;
        LastName = null!;
        Address = null!;
        CreatedAt = default;
        Status = null!;
    }

    private Order(
        string? userId,
        FirstName firstName,
        LastName lastName,
        Address address,
        DateTime createdAt,
        OrderStatus status) : base(new OrderId(Guid.NewGuid()))
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        CreatedAt = createdAt;
        Status = status;
    }

    public static Result<Order> Create(
        User user)
    {
        var order = new Order(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Address,
            DateTime.Now,
            OrderStatus.Pending);

        return order;
    }

    public static Result<Order> Create(
        FirstName firstName,
        LastName lastName,
        Address address)
    {
        var order = new Order(
            null,
            firstName,
            lastName,
            address,
            DateTime.Now,
            OrderStatus.Pending);

        return order;
    }

    public Result<OrderItemId> AddOrderItem(
        TicketPool ticketPool,
        uint quantity)
    {
        if (ticketPool.AvailableTickets < quantity)
            return Result.Failure<OrderItemId>(OrderErrors.Update.OrderItem.NotEnoughTicketsAvailable);

        if (!_orderItems.Select(x => x.Price.Currency)
                .Contains(ticketPool.Price.Currency)
            && _orderItems.Count != 0)
            return Result.Failure<OrderItemId>(OrderErrors.Update.OrderItem.CurrencyMismatch);

        var result = OrderItem.Create(
            Id,
            ticketPool.Id,
            quantity,
            ticketPool.Price);

        if (result.IsFailure)
            return Result.Failure<OrderItemId>(result.Error);

        var orderItem = result.Value();

        _orderItems.Add(orderItem);

        return Result.Success(orderItem.Id);
    }

    public Result RemoveOrderItem(
        OrderItemId orderItemId)
    {
        if (_orderItems.Count == 0)
            return Result.Failure(OrderErrors.Update.OrderItem.OrderItemNotFound);

        var orderItem = _orderItems.SingleOrDefault(oi => oi.Id == orderItemId);

        if (orderItem is null)
            return Result.Failure(OrderErrors.Update.OrderItem.OrderItemNotFound);

        _orderItems.Remove(orderItem);

        return Result.Success();
    }

    public Result UpdateOrderItemQuantity(
        OrderItemId orderItemId,
        uint quantity)
    {
        var orderItem = _orderItems.SingleOrDefault(oi => oi.Id == orderItemId);

        if (orderItem is null)
            return Result.Failure(OrderErrors.Update.OrderItem.OrderItemNotFound);

        var result = orderItem.UpdateQuantity(quantity);

        return result;
    }

    public Result<Amount> GetTotalPrice()
    {
        var amounts = OrderItems
            .Select(x => x.Price.Value * x.Quantity);

        return Amount.Create(amounts.Sum(), OrderItems.First().Price.Currency);
    }

    public Result Cancel()
    {
        if (!Status.Equals(OrderStatus.Pending))
            return Result.Failure(OrderErrors.Update.OrderItem.OrderIsNotPending);

        Status = OrderStatus.Cancelled;

        return Result.Success();
    }

    public Result Complete()
    {
        if (!Status.Equals(OrderStatus.Pending))
            return Result.Failure(OrderErrors.Update.OrderItem.OrderIsNotPending);

        foreach (var orderItem in OrderItems)
        {
            if (orderItem.TicketPool is null)
                return Result.Failure(OrderErrors.Update.OrderItem.TicketPoolNotFound);

            orderItem.TicketPool.RemoveAvailableTickets(orderItem.Quantity);
        }

        Status = OrderStatus.Completed;

        return Result.Success();
    }

    public Result Open()
    {
        if (!Status.Equals(OrderStatus.Cancelled))
            return Result.Failure(OrderErrors.Update.OrderItem.OrderIsNotCancelled);

        Status = OrderStatus.Pending;

        return Result.Success();
    }
}