using Application.Authentication.UserContext;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.EventAggregate;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.OrderItem.Add;

internal sealed class AddOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IEventRepository eventRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<AddOrderItemCommand, Result>
{
    public async Task<Result> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);
        if (order is null)
            return Result.Failure(Errors.General.EntityNotFound);

        if (order.UserId is not null)
        {
            var currentUser = userContext.GetCurrentUser();

            if (currentUser is null ||
                currentUser.Id != order.UserId)
                return Result.Failure(Errors.General.EntityNotFound);
        }

        var @event = await eventRepository.GetByIdAsync(new EventId(request.Request.EventId), cancellationToken);
        if (@event is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var ticketPool = @event.TicketPools.FirstOrDefault(x => x.Id == new TicketPoolId(request.Request.TicketPoolId));
        if (ticketPool is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var result = order.AddOrderItem(
            ticketPool,
            request.Request.Quantity);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}