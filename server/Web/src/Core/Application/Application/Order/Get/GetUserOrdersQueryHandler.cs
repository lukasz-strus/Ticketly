using Application.Authentication.UserContext;
using Application.Contracts.Order;
using Domain;
using Domain.Core.Results;
using Domain.Enums;
using Domain.OrderAggregate;
using MediatR;

namespace Application.Order.Get;

internal sealed class GetUserOrdersQueryHandler(
    IUserContext userContext,
    IOrderRepository orderRepository) : IRequestHandler<GetUserOrdersQuery, Result<OrdersResponse>>
{
    public async Task<Result<OrdersResponse>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        if (currentUser == null)
            return Result.Failure<OrdersResponse>(Errors.General.BadRequest);


        var orders = await orderRepository.GetAllByUserIdAsync(
            currentUser.Id,
            cancellationToken);

        return new OrdersResponse(
        [
            ..orders.Select(order => new OrderResponse(
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
                ])))
        ]);
    }
}