using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using MediatR;

namespace Application.Event.Event.RemoveTicketPool;

internal sealed class RemoveTicketPoolCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<RemoveTicketPoolCommand, Result>
{
    public async Task<Result> Handle(RemoveTicketPoolCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.EventId), cancellationToken);
        if (@event is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var result = @event.RemoveTicketPool(new TicketPoolId(request.TicketPoolId));

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
