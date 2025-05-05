namespace Application.Contracts.Event;

public sealed record UpdateTicketPoolRequest(
    uint AvailableTickets,
    decimal Price,
    int CurrencyId,
    DateTime StartDate,
    DateTime EndDate);
