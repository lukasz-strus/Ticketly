using Application.Contracts.Event;
using Domain.Core.Results;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Category.GetAll;

internal sealed class GetAllCategoriesQueryHandler(
    IEventRepository eventRepository) : IRequestHandler<GetAllCategoriesQuery, Result<CategoryListResponse>>
{
    public async Task<Result<CategoryListResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await eventRepository.GetAllCategoriesAsync(cancellationToken);

        return Result.Success(new CategoryListResponse(
            [
                ..categories.Select(category => new CategoryResponse(category.Id.Value, category.Name.Value))
            ]));
    }
}
