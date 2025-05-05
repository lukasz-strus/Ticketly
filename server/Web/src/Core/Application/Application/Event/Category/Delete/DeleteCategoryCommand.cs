using Domain.Core.Results;
using MediatR;

namespace Application.Event.Category.Delete;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Result>;