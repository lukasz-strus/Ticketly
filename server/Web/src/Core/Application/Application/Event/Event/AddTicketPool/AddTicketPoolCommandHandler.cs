using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.EventAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Event.Event.AddTicketPool;

internal sealed class AddTicketPoolCommandHandler(
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTicketPoolCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(AddTicketPoolCommand request,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetByIdAsync(new EventId(request.EventId), cancellationToken);
        if (@event is null)
            return Result.Failure<EntityCreatedResponse>(Errors.General.EntityNotFound);

        var currency = Currency.FromValue(request.Request.CurrencyId);
        if (currency is null)
            return Result.Failure<EntityCreatedResponse>(Errors.Enum.CurrencyNotFound);

        var amount = Amount.Create(request.Request.Price, currency);
        if (amount.IsFailure)
            return Result.Failure<EntityCreatedResponse>(amount.Error);

        var ticketPoolResult = @event.AddTicketPool(
            request.Request.AvailableTickets,
            amount.Value(),
            request.Request.StartDate,
            request.Request.EndDate);

        if (ticketPoolResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(ticketPoolResult.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(ticketPoolResult.Value().Value));
    }
}