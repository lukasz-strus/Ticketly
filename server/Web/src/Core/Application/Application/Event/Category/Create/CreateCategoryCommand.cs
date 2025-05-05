using Application.Contracts.Common;
using Application.Contracts.Event;
using Domain.Core.Results;
using MediatR;

namespace Application.Event.Category.Create;

public sealed record CreateCategoryCommand(
    CreateCategoryRequest Request) : IRequest<Result<EntityCreatedResponse>>;
