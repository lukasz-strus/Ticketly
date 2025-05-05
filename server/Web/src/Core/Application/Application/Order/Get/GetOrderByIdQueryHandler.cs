using Application.Contracts.Order;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.Get;

internal sealed class GetOrderByIdQueryHandler(
    IOrderRepository orderRepository) : IRequestHandler<GetOrderByIdQuery, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdReadOnlyAsync(new OrderId(request.Id), cancellationToken);

        return order is null
            ? Result.Failure<OrderResponse>(Errors.General.EntityNotFound)
            : Result.Success(new OrderResponse(
                order.Id.Value,
                order.FirstName.Value,
                order.LastName.Value,
                order.Address.Street,
                order.Address.Building,
                order.Address.Room,
                order.Address.Code,
                order.Address.Post,
                order.CreatedAt,
                order.Status.Value,
                order.Status.Name,
                new OrderItemsResponse(
                [
                    ..order.OrderItems.Select(x => new OrderItemResponse(
                        x.Id.Value,
                        x.OrderId.Value,
                        x.TicketPoolId.Value,
                        x.TicketPool?.Event?.Id.Value,
                        x.TicketPool?.Event?.ImageUrl,
                        x.TicketPool?.Event?.Name.Value,
                        x.Quantity,
                        x.Price.Value,
                        x.Price.Currency.Value,
                        Currency.FromValue(x.Price.Currency.Value)!.Format(x.Price.Value)))
                ])));
    }
}