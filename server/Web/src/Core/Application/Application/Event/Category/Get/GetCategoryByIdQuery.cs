using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Category.Get;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<Result<CategoryResponse>>;