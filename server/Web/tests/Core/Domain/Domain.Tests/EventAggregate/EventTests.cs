using Domain.Enums;
using Domain.EventAggregate;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.EventAggregate;

public sealed class EventTests
{
    [Fact]
    public void Create_ShouldReturnEvent()
    {
        // Arrange
        var name = Name.Create("Event Name").Value();
        var categoryId = new CategoryId(Guid.NewGuid());
        var description = Description.Create("Event Description").Value();
        var location = Address.Create("Street", "2", "2", "12345", "City").Value();
        var date = new DateTime(2025, 1, 1);
        var imageUrl = "https://example.com/image.jpg";

        // Act
        var result = Event.Create(name, categoryId, description, location, date, imageUrl);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var @event = result.Value();
        @event.Name.Should().Be(name);
        @event.CategoryId.Should().Be(categoryId);
        @event.Description.Should().Be(description);
        @event.Location.Should().Be(location);
        @event.Date.Should().Be(date);
    }

    [Fact]
    public void Update_ShouldUpdateEvent()
    {
        // Arrange
        var @event = CreateTestEvent();

        var newName = Name.Create("New Event Name").Value();
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newDescription = Description.Create("New Event Description").Value();
        var newLocation = Address.Create("New Street", "2", "2", "54321", "New City").Value();
        var newDate = new DateTime(2025, 2, 1);
        var imageUrl = "https://example.com/image.jpg";

        // Act
        var result = @event.Update(newName, newCategoryId, newDescription, newLocation, newDate, imageUrl);

        // Assert
        result.IsSuccess.Should().BeTrue();

        @event.Name.Should().Be(newName);
        @event.CategoryId.Should().Be(newCategoryId);
        @event.Description.Should().Be(newDescription);
        @event.Location.Should().Be(newLocation);
        @event.Date.Should().Be(newDate);
    }

    [Fact]
    public void Update_WithNewDateLowerThanMaxSaleEndDate_ShouldNotUpdateEvent()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);
        @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        var newName = Name.Create("New Event Name").Value();
        var newCategoryId = new CategoryId(Guid.NewGuid());
        var newDescription = Description.Create("New Event Description").Value();
        var newLocation = Address.Create("New Street", "2", "2", "54321", "New City").Value();
        var newDate = new DateTime(2021, 2, 1);
        var imageUrl = "https://example.com/image.jpg";


        // Act
        var result = @event.Update(newName, newCategoryId, newDescription, newLocation, newDate, imageUrl);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(EventErrors.EventDateMustBeGreaterThanMaxTicketPoolSaleDate);

        @event.Name.Should().NotBe(newName);
        @event.CategoryId.Should().NotBe(newCategoryId);
        @event.Description.Should().NotBe(newDescription);
        @event.Location.Should().NotBe(newLocation);
        @event.Date.Should().NotBe(newDate);
    }

    [Fact]
    public void AddTicketPool_ShouldAddTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        // Act
        var result = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var ticketPoolId = result.Value();
        @event.TicketPools.Should().HaveCount(1);
        @event.TicketPools.Single().Id.Should().Be(ticketPoolId);
    }

    [Fact]
    public void AddTicketPool_WithSaleStartDateGreaterThanSaleEndDate_ShouldNotAddTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 12, 1);
        var saleEnd = new DateTime(2024, 1, 1);

        // Act
        var result = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void AddTicketPool_WithSaleStartDateGreaterThanEventDate_ShouldNotAddTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = @event.Date.AddDays(1);
        var saleEnd = @event.Date.AddDays(2);

        // Act
        var result = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void AddTicketPool_WithSaleEndDateGreaterThanEventDate_ShouldNotAddTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 12, 1);
        var saleEnd = @event.Date.AddDays(1);

        // Act
        var result = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void RemoveTicketPool_ShouldRemoveTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        var addTicketPoolResult = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var ticketPoolId = addTicketPoolResult.Value();

        // Act
        var result = @event.RemoveTicketPool(ticketPoolId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        @event.TicketPools.Should().BeEmpty();
    }

    [Fact]
    public void RemoveTicketPool_WithWrongTicketPoolId_ShouldNotRemoveTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);

        // Act
        var result = @event.RemoveTicketPool(new TicketPoolId(Guid.NewGuid()));

        // Assert
        result.IsFailure.Should().BeTrue();
        @event.TicketPools.Should().HaveCount(1);
    }


    [Fact]
    public void UpdateTicketPool_ShouldUpdateTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        var addTicketPoolResult = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var ticketPoolId = addTicketPoolResult.Value();
        const uint newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = new DateTime(2024, 5, 1);
        var newSaleEnd = new DateTime(2024, 6, 1);

        // Act
        var result = @event.UpdateTicketPool(ticketPoolId, newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updatedTicketPool = @event.TicketPools.Single(tp => tp.Id == ticketPoolId);
        updatedTicketPool.AvailableTickets.Should().Be(newAvailableTickets);
        updatedTicketPool.Price.Should().Be(newPrice);
        updatedTicketPool.SaleStart.Should().Be(newSaleStart);
        updatedTicketPool.SaleEnd.Should().Be(newSaleEnd);
    }

    [Fact]
    public void UpdateTicketPool_WithSaleStartDateGreaterThanSaleEndDate_ShouldNotUpdateTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        var addTicketPoolResult = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var ticketPoolId = addTicketPoolResult.Value();
        const uint newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = new DateTime(2024, 12, 1);
        var newSaleEnd = new DateTime(2024, 1, 1);

        // Act
        var result = @event.UpdateTicketPool(ticketPoolId, newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();

        var updatedTicketPool = @event.TicketPools.Single(tp => tp.Id == ticketPoolId);
        updatedTicketPool.Should().NotBeNull();
        updatedTicketPool.AvailableTickets.Should().Be(availableTickets);
        updatedTicketPool.Price.Should().Be(price);
        updatedTicketPool.SaleStart.Should().Be(saleStart);
        updatedTicketPool.SaleEnd.Should().Be(saleEnd);
    }

    [Fact]
    public void UpdateTicketPool_WithSaleStartDateGreaterThanEventDate_ShouldNotUpdateTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        var addTicketPoolResult = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var ticketPoolId = addTicketPoolResult.Value();
        const uint newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = @event.Date.AddDays(1);
        var newSaleEnd = @event.Date.AddDays(2);

        // Act
        var result = @event.UpdateTicketPool(ticketPoolId, newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();

        var updatedTicketPool = @event.TicketPools.Single(tp => tp.Id == ticketPoolId);
        updatedTicketPool.Should().NotBeNull();
        updatedTicketPool.AvailableTickets.Should().Be(availableTickets);
        updatedTicketPool.Price.Should().Be(price);
        updatedTicketPool.SaleStart.Should().Be(saleStart);
        updatedTicketPool.SaleEnd.Should().Be(saleEnd);
    }

    [Fact]
    public void UpdateTicketPool_WithSaleEndDateGreaterThanEventDate_ShouldNotUpdateTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);

        var addTicketPoolResult = @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var ticketPoolId = addTicketPoolResult.Value();
        const uint newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = new DateTime(2024, 12, 1);
        var newSaleEnd = @event.Date.AddDays(1);

        // Act
        var result = @event.UpdateTicketPool(ticketPoolId, newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();

        var updatedTicketPool = @event.TicketPools.Single(tp => tp.Id == ticketPoolId);
        updatedTicketPool.Should().NotBeNull();
        updatedTicketPool.AvailableTickets.Should().Be(availableTickets);
        updatedTicketPool.Price.Should().Be(price);
        updatedTicketPool.SaleStart.Should().Be(saleStart);
        updatedTicketPool.SaleEnd.Should().Be(saleEnd);
    }

    [Fact]
    public void UpdateTicketPool_WithWrongTicketPoolId_ShouldNotUpdateTicketPool()
    {
        // Arrange
        var @event = CreateTestEvent();
        const uint availableTickets = 100u;
        var price = Amount.Create(50, Currency.Usd).Value();
        var saleStart = new DateTime(2024, 1, 1);
        var saleEnd = new DateTime(2024, 12, 1);
        @event.AddTicketPool(availableTickets, price, saleStart, saleEnd);
        var wrongTicketPoolId = new TicketPoolId(Guid.NewGuid());
        const uint newAvailableTickets = 200u;
        var newPrice = Amount.Create(100, Currency.Usd).Value();
        var newSaleStart = new DateTime(2024, 5, 1);
        var newSaleEnd = new DateTime(2024, 6, 1);

        // Act
        var result =
            @event.UpdateTicketPool(wrongTicketPoolId, newAvailableTickets, newPrice, newSaleStart, newSaleEnd);

        // Assert
        result.IsFailure.Should().BeTrue();

        @event.TicketPools.Should().HaveCount(1);
    }


    private static Event CreateTestEvent()
    {
        var name = Name.Create("Event Name").Value();
        var categoryId = new CategoryId(Guid.NewGuid());
        var description = Description.Create("Event Description").Value();
        var location = Address.Create("Street", "1", "1", "12345", "City").Value();
        var date = new DateTime(2025, 1, 1);
        var imageUrl = "https://example.com/image.jpg";

        var result = Event.Create(name, categoryId, description, location, date, imageUrl);

        return result.Value();
    }
}