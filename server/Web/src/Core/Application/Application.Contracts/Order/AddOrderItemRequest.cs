namespace Application.Contracts.Order;

public sealed record AddOrderItemRequest(
    Guid EventId,
    Guid TicketPoolId,
    uint Quantity);