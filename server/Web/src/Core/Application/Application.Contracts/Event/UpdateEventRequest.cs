namespace Application.Contracts.Event;

public sealed record UpdateEventRequest(
    string Name,
    Guid CategoryId,
    string Description,
    string LocationStreet,
    string LocationBuilding,
    string? LocationRoom,
    string LocationCode,
    string LocationPost,
    DateTime Date,
    string? ImageUrl);