using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.Status;

internal sealed class OpenOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<OpenOrderCommand, Result>
{
    public async Task<Result> Handle(OpenOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(new OrderId(request.OrderId), cancellationToken);
        if (order is null)
            return Result.Failure(Errors.General.EntityNotFound);

        var result = order.Open();
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

//TODO: AddItem, UpdateItem, RemoveItem. Zrobić też tak, że przy pobierniau sprawdza aktualną cenę ticket poola, oraz czy są dostępne.