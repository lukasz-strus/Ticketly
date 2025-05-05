using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.Status;

internal sealed class CompleteOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CompleteOrderCommand, Result>
{
    public async Task<Result> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);
        if (order is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var result = order.Complete();
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}