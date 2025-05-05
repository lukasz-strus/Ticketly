using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.EventAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Event.Event.UpdateTicketPool;

internal sealed class UpdateTicketPoolCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTicketPoolCommand, Result>
{
    public async Task<Result> Handle(UpdateTicketPoolCommand request, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.EventId), cancellationToken);
        if (@event is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var currency = Currency.FromValue(request.Request.CurrencyId);
        if (currency is null)
            return Result.Failure(Errors.Enum.CurrencyNotFound);

        var amount = Amount.Create(request.Request.Price, currency);
        if (amount.IsFailure)
            return Result.Failure(amount.Error);

        var result = @event.UpdateTicketPool(
            new TicketPoolId(request.TicketPoolId),
            request.Request.AvailableTickets,
            amount.Value(),
            request.Request.StartDate,
            request.Request.EndDate);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
