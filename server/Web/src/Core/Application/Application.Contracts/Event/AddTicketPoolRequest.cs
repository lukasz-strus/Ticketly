namespace Application.Contracts.Event;

public sealed record AddTicketPoolRequest(
    uint AvailableTickets,
    decimal Price,
    int CurrencyId,
    DateTime StartDate,
    DateTime EndDate);
