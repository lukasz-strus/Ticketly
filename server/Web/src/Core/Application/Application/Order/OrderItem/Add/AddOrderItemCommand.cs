using Application.Contracts.Order;
using Domain.Core.Results;
using MediatR;

namespace Application.Order.OrderItem.Add;

public sealed record AddOrderItemCommand(
    Guid OrderId,
    AddOrderItemRequest Request) : IRequest<Result>;