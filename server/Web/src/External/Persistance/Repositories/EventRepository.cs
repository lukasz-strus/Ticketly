using Domain.EventAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

internal sealed class EventRepository(
    ApplicationDbContext dbContext) : IEventRepository
{
    public async Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default) =>
        await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Category?> GetByIdReadOnlyAsync(
        CategoryId id,
        CancellationToken cancellationToken = default) =>
        await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Event?> GetByIdAsync(EventId id, CancellationToken cancellationToken = default) =>
        await dbContext.Events
            .Include(x => x.Category)
            .Include(x => x.TicketPools)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Event?>
        GetByIdReadOnlyAsync(EventId id, CancellationToken cancellationToken = default) =>
        await dbContext.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<List<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Events
            .Include(x => x.Category)
            .Include(x => x.TicketPools)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<List<Event>> GetAllEventsAsync(CategoryId categoryId,
        CancellationToken cancellationToken = default) =>
        await dbContext.Events
            .Where(x => x.CategoryId == categoryId)
            .Include(x => x.Category)
            .Include(x => x.TicketPools)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Category category) =>
        await dbContext.Categories.AddAsync(category);

    public async Task AddAsync(Event @event) =>
        await dbContext.Events.AddAsync(@event);

    public void Delete(Category category) =>
        dbContext.Categories.Remove(category);

    public void Delete(Event @event) =>
        dbContext.Events.Remove(@event);

    public Task<bool> CategoryNameExistsAsync(Name name) =>
        dbContext.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Name == name);

    public Task<bool> EventNameExistsAsync(Name name) =>
        dbContext.Events
            .AsNoTracking()
            .AnyAsync(x => x.Name == name);
}