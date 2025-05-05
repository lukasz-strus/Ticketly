namespace Application.Contracts.Order;

public sealed record OrderItemResponse(
    Guid Id,
    Guid OrderId,
    Guid TicketPoolId,
    Guid? EventId,
    string? EventImgUrl,
    string? EventName,
    uint Quantity,
    decimal PriceAmount,
    int PriceCurrencyId,
    string Price);