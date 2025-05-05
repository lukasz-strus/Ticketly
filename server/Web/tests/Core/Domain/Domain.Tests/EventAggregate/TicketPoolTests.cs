using Domain.Enums;
using Domain.EventAggregate;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.EventAggregate;

public sealed class TicketPoolTests
{
    [Fact]
    public void Create_ShouldReturnSuccessResultWithTicketPool()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = DateTime.Now;
        var saleEnd = DateTime.Now.AddDays(1);

        // Act
        var result = TicketPool.Create(eventId, availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var ticketPool = result.Value();
        ticketPool.Should().NotBeNull();
        ticketPool.EventId.Should().Be(eventId);
        ticketPool.AvailableTickets.Should().Be(availableTickets);
        ticketPool.Price.Should().Be(price);
        ticketPool.SaleStart.Should().Be(saleStart);
        ticketPool.SaleEnd.Should().Be(saleEnd);
    }

    [Fact]
    public void Create_WithSaleStartGreaterThanSaleEnd_ShouldReturnFailureResult()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = DateTime.Now.AddDays(1);
        var saleEnd = DateTime.Now;

        // Act
        var result = TicketPool.Create(eventId, availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Update_ShouldUpdateTicketPool()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var createResult = TicketPool.Create(
            eventId, 100,
            Amount.Create(50, Currency.Usd).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));

        var ticketPool = createResult.Value();
        var newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = DateTime.Now;
        var newSaleEnd = DateTime.Now.AddDays(2);

        // Act
        var result = ticketPool.Update(newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ticketPool.AvailableTickets.Should().Be(newAvailableTickets);
        ticketPool.Price.Should().Be(newPrice);
        ticketPool.SaleStart.Should().Be(newSaleStart);
        ticketPool.SaleEnd.Should().Be(newSaleEnd);
    }


    [Fact]
    public void Update_WithSaleStartGreaterThanSaleEnd_ShouldReturnFailureResult()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var createResult = TicketPool.Create(
            eventId, 100,
            Amount.Create(50, Currency.Usd).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));
        var ticketPool = createResult.Value();
        var newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = DateTime.Now.AddDays(1);
        var newSaleEnd = DateTime.Now;

        // Act
        var result = ticketPool.Update(newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RemoveAvailableTickets_ShouldDecreaseAvailableTickets()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var createResult = TicketPool.Create(
            eventId,
            100,
            Amount.Create(50, Currency.Usd).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));
        var ticketPool = createResult.Value();
        var ticketsToRemove = 50u;

        // Act
        var result = ticketPool.RemoveAvailableTickets(ticketsToRemove);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ticketPool.AvailableTickets.Should().Be(50);
    }

    [Fact]
    public void RemoveAvailableTickets_ShouldReturnFailureWhenNotEnoughTickets()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var createResult = TicketPool.Create(
            eventId,
            100,
            Amount.Create(50, Currency.Usd).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));
        var ticketPool = createResult.Value();
        var ticketsToRemove = 150u;

        // Act
        var result = ticketPool.RemoveAvailableTickets(ticketsToRemove);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(EventErrors.TicketPools.NotEnoughTicketsAvailable);
    }

    [Fact]
    public void AddAvailableTickets_ShouldIncreaseAvailableTickets()
    {
        // Arrange
        var eventId = new EventId(Guid.NewGuid());
        var createResult = TicketPool.Create(
            eventId,
            100,
            Amount.Create(50, Currency.Usd).Value(),
            DateTime.Now,
            DateTime.Now.AddDays(1));
        var ticketPool = createResult.Value();
        var ticketsToAdd = 50u;

        // Act
        var result = ticketPool.AddAvailableTickets(ticketsToAdd);

        // Assert
        result.IsSuccess.Should().BeTrue();
        ticketPool.AvailableTickets.Should().Be(150);
    }
}