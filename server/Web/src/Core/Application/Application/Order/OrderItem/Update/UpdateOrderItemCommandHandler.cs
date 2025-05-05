using Application.Authentication.UserContext;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.OrderItem.Update;

internal sealed class UpdateOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrderItemCommand, Result>
{
    public async Task<Result> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
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

        var result = order.UpdateOrderItemQuantity(
            new OrderItemId(request.OrderItemId),
            request.Request.Quantity);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}