namespace Domain.OrderAggregate;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(OrderId orderId, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdReadOnlyAsync(OrderId orderId, CancellationToken cancellationToken = default);
    Task<Order?> GetPendingByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Order?> GetPendingByUserIdReadOnlyAsync(string userId, CancellationToken cancellationToken = default);

    Task<List<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<List<Order>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}