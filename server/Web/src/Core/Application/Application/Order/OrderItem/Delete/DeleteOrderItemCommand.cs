using Domain.Core.Results;
using MediatR;

namespace Application.Order.OrderItem.Delete;

public sealed record DeleteOrderItemCommand(
    Guid OrderId,
    Guid OrderItemId) : IRequest<Result>;