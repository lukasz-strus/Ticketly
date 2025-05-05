namespace Application.Contracts.Order;

public sealed record OrdersResponse(IReadOnlyCollection<OrderResponse> Items);