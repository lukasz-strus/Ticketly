using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Event.Event.Create;

internal sealed class CreateEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateEventCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateEventCommand request,
        CancellationToken cancellationToken)
    {
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

        var eventResult = Domain.EventAggregate.Event.Create(
            nameResult.Value(),
            category.Id,
            descriptionResult.Value(),
            locationResult.Value(),
            date,
            request.Request.ImageUrl);

        if (eventResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(eventResult.Error);

        var @event = eventResult.Value();

        await eventRepository.AddAsync(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(@event.Id.Value));
    }
}