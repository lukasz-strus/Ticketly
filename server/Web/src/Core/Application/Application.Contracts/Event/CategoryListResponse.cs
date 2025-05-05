namespace Application.Contracts.Event;

public sealed record CategoryListResponse(IReadOnlyCollection<CategoryResponse> Items);
