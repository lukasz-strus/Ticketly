using Domain.Enums;
using Domain.EventAggregate;
using Domain.OrderAggregate;
using Domain.UserAggregate;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.OrderAggregate;

public sealed class OrderTests
{
    [Fact]
    public void Create_WithUser_ShouldReturnOrder()
    {
        // Arrange
        var user = CreateUser();

        // Act
        var result = Order.Create(user);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var order = result.Value();
        order.UserId.Should().Be(user.Id);
        order.Status.Should().Be(OrderStatus.Pending);
        order.OrderItems.Should().BeEmpty();
    }

    [Fact]
    public void Create_WithoutUser_ShouldReturnOrder()
    {
        // Arrange
        var firstName = FirstName.Create("First name").Value();
        var lastName = LastName.Create("Last name").Value();
        var address = Address.Create("Street", "Building", "1", "00001", "Post").Value();

        // Act
        var result = Order.Create(firstName, lastName, address);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var order = result.Value();
        order.UserId.Should().BeNull();
        order.Status.Should().Be(OrderStatus.Pending);
        order.OrderItems.Should().BeEmpty();
    }

    [Fact]
    public void AddOrderItem_ShouldAddOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;

        // Act
        var result = order.AddOrderItem(ticketPool, quantity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.OrderItems.Should().HaveCount(1);
    }

    [Fact]
    public void AddOrderItem_WithNotEnoughTickets_ShouldNotAddOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 200;

        // Act
        var result = order.AddOrderItem(ticketPool, quantity);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void AddOrderItem_WithDifferentCurrency_ShouldNotAddOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        order.AddOrderItem(ticketPool, quantity);
        var ticketCreateResult = TicketPool.Create(
            new EventId(Guid.NewGuid()),
            100,
            Amount.Create(100m, Currency.Eur).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));
        var ticketPoolWithDifferentCurrency = ticketCreateResult.Value();

        // Act
        var result = order.AddOrderItem(ticketPoolWithDifferentCurrency, quantity);
        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RemoveOrderItem_ShouldRemoveOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        var addOrderItemResult = order.AddOrderItem(ticketPool, quantity);
        var orderItemId = addOrderItemResult.Value();

        // Act
        var result = order.RemoveOrderItem(orderItemId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.OrderItems.Should().BeEmpty();
    }

    [Fact]
    public void RemoveOrderItem_WithWrongOrderItemId_ShouldNotRemoveOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        order.AddOrderItem(ticketPool, quantity);
        var wrongOrderItemId = new OrderItemId(Guid.NewGuid());

        // Act
        var result = order.RemoveOrderItem(wrongOrderItemId);

        // Assert
        result.IsFailure.Should().BeTrue();
        order.OrderItems.Should().HaveCount(1);
    }

    [Fact]
    public void RemoveOrderItem_WithEmptyOrderItems_ShouldNotRemoveOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var orderItemId = new OrderItemId(Guid.NewGuid());

        // Act
        var result = order.RemoveOrderItem(orderItemId);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void UpdateOrderItemQuantity_ShouldUpdateOrderItemQuantity()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        var addOrderItemResult = order.AddOrderItem(ticketPool, quantity);
        var orderItemId = addOrderItemResult.Value();
        const uint newQuantity = 5;

        // Act
        var result = order.UpdateOrderItemQuantity(orderItemId, newQuantity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updatedOrderItem = order.OrderItems.Single(oi => oi.Id == orderItemId);
        updatedOrderItem.Quantity.Should().Be(newQuantity);
    }

    [Fact]
    public void UpdateOrderItemQuantity_WithWrongId_ShouldNotUpdateOrderItem()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        var addOrderItemResult = order.AddOrderItem(ticketPool, quantity);
        var orderItemId = addOrderItemResult.Value();
        const uint newQuantity = 5;
        var wrongOrderItemId = new OrderItemId(Guid.NewGuid());

        // Act
        var result = order.UpdateOrderItemQuantity(wrongOrderItemId, newQuantity);

        // Assert
        result.IsFailure.Should().BeTrue();
        var updatedOrderItem = order.OrderItems.Single(oi => oi.Id == orderItemId);
        updatedOrderItem.Quantity.Should().Be(quantity);
    }

    [Fact]
    public void GetTotalPrice_ShouldReturnTotalPrice()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        order.AddOrderItem(ticketPool, quantity);

        // Act
        var result = order.GetTotalPrice();

        // Assert
        result.IsSuccess.Should().BeTrue();
        var totalPrice = result.Value();
        totalPrice.Should().NotBeNull();
        totalPrice.Value.Should().Be(ticketPool.Price.Value * quantity);
    }

    [Fact]
    public void Cancel_ShouldCancelOrder()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        var result = order.Cancel();

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void Cancel_WhenOrderIsNotPending_ShouldNotCancel()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Complete();

        // Act
        var result = order.Cancel();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Complete_ShouldCompleteOrder()
    {
        // Arrange
        var order = CreateTestOrder();
        var ticketPool = CreateTestTicketPool();
        const uint quantity = 2;
        order.AddOrderItem(ticketPool, quantity);

        var ticketPoolProperty = typeof(OrderItem).GetProperty("TicketPool");
        ticketPoolProperty?.SetValue(order.OrderItems.First(), ticketPool);

        // Act
        var result = order.Complete();

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Completed);
    }

    [Fact]
    public void Complete_WhenOrderIsNotPending_ShouldNotComplete()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Cancel();

        // Act
        var result = order.Complete();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Open_ShouldOpenOrder()
    {
        // Arrange
        var order = CreateTestOrder();
        order.Cancel();

        // Act
        var result = order.Open();

        // Assert
        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Pending);
    }

    [Fact]
    public void Open_WhenOrderIsNotCancelled_ShouldNotOpen()
    {
        // Arrange
        var order = CreateTestOrder();

        // Act
        var result = order.Open();

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    private static Order CreateTestOrder()
    {
        var user = CreateUser();
        var result = Order.Create(user);
        return result.Value();
    }

    private static User CreateUser()
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = FirstName.Create("First name").Value(),
            LastName = LastName.Create("Last name").Value(),
            Address = Address.Create("Street", "Building", "1", "00001", "Post").Value()
        };
    }

    private static TicketPool CreateTestTicketPool()
    {
        var eventId = new EventId(Guid.NewGuid());
        var availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = DateTime.Now;
        var saleEnd = DateTime.Now.AddDays(1);
        var result = TicketPool.Create(eventId, availableTickets, price, saleStart, saleEnd);
        return result.Value();
    }
}