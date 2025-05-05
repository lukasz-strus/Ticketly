using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Category.Delete;

internal sealed class DeleteCategoryCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await eventRepository.GetByIdAsync(new CategoryId(request.Id), cancellationToken);
        if (category is null)
            return Result.Failure(Errors.General.EntityNotFound);

        eventRepository.Delete(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
