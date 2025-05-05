using Application.Authentication.UserContext;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.OrderItem.Delete;

internal sealed class DeleteOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderItemCommand, Result>
{
    public async Task<Result> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
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

        var result = order.RemoveOrderItem(new OrderItemId(request.OrderItemId));
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}