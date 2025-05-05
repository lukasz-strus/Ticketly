using Domain.Enums;
using Domain.EventAggregate;
using Domain.OrderAggregate;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.OrderAggregate;

public sealed class OrderItemTests
{
    [Fact]
    public void Create_ShouldReturnSuccessResultWithOrderItem()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var ticketPoolId = new TicketPoolId(Guid.NewGuid());
        var quantity = 2u;
        var price = Amount.Create(100, Currency.Usd).Value();

        // Act
        var result = OrderItem.Create(orderId, ticketPoolId, quantity, price);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var orderItem = result.Value();
        orderItem.Should().NotBeNull();
        orderItem.OrderId.Should().Be(orderId);
        orderItem.TicketPoolId.Should().Be(ticketPoolId);
        orderItem.Quantity.Should().Be(quantity);
        orderItem.Price.Should().Be(price);
    }

    [Fact]
    public void UpdateQuantity_ShouldUpdateOrderItemQuantity()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());
        var ticketPoolId = new TicketPoolId(Guid.NewGuid());
        var createResult = OrderItem.Create(orderId, ticketPoolId, 2, Amount.Create(100, Currency.Usd).Value());
        var orderItem = createResult.Value();
        var newQuantity = 5u;

        // Act
        var result = orderItem.UpdateQuantity(newQuantity);

        // Assert
        result.IsSuccess.Should().BeTrue();
        orderItem.Quantity.Should().Be(newQuantity);
    }
}
