namespace Application.Contracts.Event;

public sealed record TicketPoolResponse(
    Guid Id,
    uint AvailableTickets,
    decimal PriceAmount,
    int PriceCurrencyId,
    string Price,
    DateTime StartDate,
    DateTime EndDate);