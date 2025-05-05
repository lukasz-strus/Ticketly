namespace Application.Contracts.Event;

public sealed record EventListResponse(IReadOnlyCollection<EventResponse> Items);
