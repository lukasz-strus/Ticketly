namespace Application.Contracts.Event;

public sealed record EventResponse(
    Guid Id,
    string Name,
    Guid CategoryId,
    string? CategoryName,
    string Description,
    string LocationStreet,
    string LocationBuilding,
    string? LocationRoom,
    string LocationCode,
    string LocationPost,
    DateTime Date,
    string? ImageUrl,
    TicketPoolListResponse TicketPools);