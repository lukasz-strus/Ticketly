using Application.Contracts.Order;
using Domain.Core.Results;
using MediatR;

namespace Application.Order.OrderItem.Update;

public sealed record UpdateOrderItemCommand(
    Guid OrderId,
    Guid OrderItemId,
    UpdateOrderItemRequest Request) : IRequest<Result>;