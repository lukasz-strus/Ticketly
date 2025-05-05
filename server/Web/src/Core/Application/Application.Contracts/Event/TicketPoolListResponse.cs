namespace Application.Contracts.Event;

public sealed record TicketPoolListResponse(IReadOnlyCollection<TicketPoolResponse> Items);