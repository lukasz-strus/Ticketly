using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Event.Category.Create;

internal sealed class CreateCategoryCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(request.Request.Name);
        if(nameResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(nameResult.Error);

        var name = nameResult.Value();

        if(await eventRepository.CategoryNameExistsAsync(name))
            return Result.Failure<EntityCreatedResponse>(EventErrors.Category.NameAlreadyExists);

        var categoryResult = Domain.EventAggregate.Category.Create(name);

        if (categoryResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(categoryResult.Error);

        var category = categoryResult.Value();

        await eventRepository.AddAsync(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(category.Id.Value));
    }
}
