using Domain.ValueObjects;

namespace Domain.EventAggregate;

public interface IEventRepository
{
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdReadOnlyAsync(CategoryId id, CancellationToken cancellationToken = default);

    Task<Event?> GetByIdAsync(EventId id, CancellationToken cancellationToken = default);
    Task<Event?> GetByIdReadOnlyAsync(EventId id, CancellationToken cancellationToken = default);

    Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<Event>> GetAllEventsAsync(CancellationToken cancellationToken = default);
    Task<List<Event>> GetAllEventsAsync(CategoryId categoryId, CancellationToken cancellationToken = default);

    Task AddAsync(Category category);
    Task AddAsync(Event @event);

    void Delete(Category category);
    void Delete(Event @event);

    Task<bool> CategoryNameExistsAsync(Name name);
    Task<bool> EventNameExistsAsync(Name name);
}