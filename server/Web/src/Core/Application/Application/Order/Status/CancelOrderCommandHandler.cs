using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.Status;

internal sealed class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);
        if (order is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var result = order.Cancel();
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}