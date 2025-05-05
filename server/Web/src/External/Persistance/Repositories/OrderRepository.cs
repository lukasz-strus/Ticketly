using Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

internal sealed class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default) =>
        await dbContext.Orders.AddAsync(order, cancellationToken);

    public async Task<Order?> GetByIdAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .Include(x => x.OrderItems).ThenInclude(x => x.TicketPool).ThenInclude(x => x!.Event)
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

    public async Task<Order?> GetByIdReadOnlyAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .Include(x => x.OrderItems).ThenInclude(x => x.TicketPool).ThenInclude(x => x!.Event)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

    public async Task<Order?> GetPendingByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .Include(x => x.OrderItems).ThenInclude(x => x.TicketPool).ThenInclude(x => x!.Event)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == OrderStatus.Pending, cancellationToken);

    public async Task<Order?> GetPendingByUserIdReadOnlyAsync(string userId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .Include(x => x.OrderItems).ThenInclude(x => x.TicketPool).ThenInclude(x => x!.Event)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == OrderStatus.Pending, cancellationToken);

    public async Task<List<Order>> GetAllOrdersAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<List<Order>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default) =>
        await dbContext.Orders
            .Include(x => x.OrderItems).ThenInclude(x => x.TicketPool).ThenInclude(x => x!.Event)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
}