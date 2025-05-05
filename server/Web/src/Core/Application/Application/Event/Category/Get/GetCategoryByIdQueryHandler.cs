using Application.Contracts.Event;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Category.Get;

internal sealed class GetCategoryByIdQueryHandler(
    IEventRepository eventRepository) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await eventRepository.GetByIdReadOnlyAsync(
            new CategoryId(request.Id),
            cancellationToken);

        return category is null
            ? Result.Failure<CategoryResponse>(Errors.General.EntityNotFound)
            : Result.Success(new CategoryResponse(category.Id.Value, category.Name.Value));
    }
}