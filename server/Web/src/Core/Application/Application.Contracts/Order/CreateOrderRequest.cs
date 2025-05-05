namespace Application.Contracts.Order;

public sealed record CreateOrderRequest(
    string? FirstName,
    string? LastName,
    string? AddressStreet,
    string? AddressBuilding,
    string? AddressRoom,
    string? AddressCode,
    string? AddressPost);