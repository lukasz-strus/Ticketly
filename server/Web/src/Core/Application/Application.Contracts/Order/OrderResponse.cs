namespace Application.Contracts.Order;

public sealed record OrderResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string AddressStreet,
    string AddressBuilding,
    string? AddressRoom,
    string AddressCode,
    string AddressPost,
    DateTime CreatedAt,
    int StatusId,
    string Status,
    OrderItemsResponse OrderItems);