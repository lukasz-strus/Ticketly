using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Event.Delete;

internal sealed class DeleteEventCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventCommand, Result>
{
    public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.Id), cancellationToken);
        if (@event is null)
            return Result.Failure(Errors.General.EntityNotFound);

        eventRepository.Delete(@event);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
