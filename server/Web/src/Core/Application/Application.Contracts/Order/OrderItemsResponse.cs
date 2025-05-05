namespace Application.Contracts.Order;

public sealed record OrderItemsResponse(IReadOnlyCollection<OrderItemResponse> Items);