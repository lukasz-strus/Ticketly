using System.Diagnostics.CodeAnalysis;
using Domain.Core.Primitives;
using Domain.Core.Results;
using Domain.ValueObjects;

namespace Domain.EventAggregate;

public class Event : AggregateRoot<EventId>
{
    private readonly HashSet<TicketPool> _ticketPools = [];

    public Name Name { get; private set; }
    public CategoryId CategoryId { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    [ExcludeFromCodeCoverage] public Category? Category { get; private set; }
    public Description Description { get; private set; }
    public Address Location { get; private set; }
    public DateTime Date { get; private set; }
    public string? ImageUrl { get; private set; }

    public IReadOnlyCollection<TicketPool> TicketPools => _ticketPools;

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    [ExcludeFromCodeCoverage]
    private Event()
    {
        Name = null!;
        CategoryId = null!;
        Description = null!;
        Location = null!;
        Date = default;
    }

    private Event(
        Name name,
        CategoryId categoryId,
        Description description,
        Address location,
        DateTime date,
        string? imageUrl) : base(new EventId(Guid.NewGuid()))
    {
        Name = name;
        CategoryId = categoryId;
        Description = description;
        Location = location;
        Date = date;
        ImageUrl = imageUrl;
    }

    public static Result<Event> Create(
        Name name,
        CategoryId categoryId,
        Description description,
        Address location,
        DateTime date,
        string? imageUrl)
    {
        var @event = new Event(name, categoryId, description, location, date, imageUrl);

        return @event;
    }

    public Result Update(
        Name name,
        CategoryId categoryId,
        Description description,
        Address location,
        DateTime date,
        string? imageUrl)
    {
        var maxTicketPoolSaleEndDate = TicketPools
            .Select(tp => tp.SaleEnd)
            .DefaultIfEmpty(DateTime.MinValue)
            .Max();

        if (date < maxTicketPoolSaleEndDate)
            return Result.Failure(EventErrors.EventDateMustBeGreaterThanMaxTicketPoolSaleDate);

        Name = name;
        CategoryId = categoryId;
        Description = description;
        Location = location;
        Date = date;
        ImageUrl = imageUrl;

        return Result.Success();
    }

    public Result<TicketPoolId> AddTicketPool(
        uint availableTickets,
        Amount price,
        DateTime saleStart,
        DateTime saleEnd)
    {
        if (saleStart > Date)
            return Result.Failure<TicketPoolId>(
                EventErrors.TicketPools.TicketPoolStartSaleDateMustBeLessThanEventDate);

        if (saleEnd > Date)
            return Result.Failure<TicketPoolId>(
                EventErrors.TicketPools.TicketPoolEndSaleDateMustBeLessThanEventDate);

        var result = TicketPool.Create(Id, availableTickets, price, saleStart, saleEnd);

        if (result.IsFailure)
            return Result.Failure<TicketPoolId>(result.Error);

        var ticketPool = result.Value();

        _ticketPools.Add(ticketPool);

        return ticketPool.Id;
    }

    public Result RemoveTicketPool(TicketPoolId ticketPoolId)
    {
        var ticketPool = _ticketPools.SingleOrDefault(tp => tp.Id == ticketPoolId);

        if (ticketPool is null)
            return Result.Failure(EventErrors.TicketPools.TicketPoolNotFound);

        _ticketPools.Remove(ticketPool);

        return Result.Success();
    }

    public Result UpdateTicketPool(
        TicketPoolId ticketPoolId,
        uint availableTickets,
        Amount price,
        DateTime saleStart,
        DateTime saleEnd)
    {
        if (saleStart > Date)
            return Result.Failure<TicketPoolId>(
                EventErrors.TicketPools.TicketPoolStartSaleDateMustBeLessThanEventDate);

        if (saleEnd > Date)
            return Result.Failure<TicketPoolId>(
                EventErrors.TicketPools.TicketPoolEndSaleDateMustBeLessThanEventDate);

        var ticketPool = _ticketPools.SingleOrDefault(tp => tp.Id == ticketPoolId);

        if (ticketPool is null)
            return Result.Failure(EventErrors.TicketPools.TicketPoolNotFound);

        var result = ticketPool.Update(availableTickets, price, saleStart, saleEnd);

        return result;
    }
}