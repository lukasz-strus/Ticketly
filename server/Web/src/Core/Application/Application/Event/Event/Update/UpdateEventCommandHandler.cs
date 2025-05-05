using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Event.Event.Update;

internal sealed class UpdateEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateEventCommand, Result>
{
    public async Task<Result> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.Id), cancellationToken);
        if (@event is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var nameResult = Name.Create(request.Request.Name);
        if (nameResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(nameResult.Error);

        var category =
            await eventRepository.GetByIdAsync(new CategoryId(request.Request.CategoryId), cancellationToken);
        if (category is null)
            return Result.Failure<EntityCreatedResponse>(Errors.General.EntityNotFound);

        var descriptionResult = Description.Create(request.Request.Description);
        if (descriptionResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(descriptionResult.Error);

        var locationResult = Address.Create(
            request.Request.LocationStreet,
            request.Request.LocationBuilding,
            request.Request.LocationRoom,
            request.Request.LocationCode,
            request.Request.LocationPost);

        if (locationResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(locationResult.Error);

        var date = request.Request.Date;

        var result = @event.Update(
            nameResult.Value(),
            category.Id,
            descriptionResult.Value(),
            locationResult.Value(),
            date,
            request.Request.ImageUrl);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}