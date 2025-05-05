namespace Application.Contracts.Order;

public sealed record UpdateOrderItemRequest(
    uint Quantity);