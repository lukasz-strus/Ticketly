using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Category.GetAll;

public sealed record GetAllCategoriesQuery : IRequest<Result<CategoryListResponse>>;
