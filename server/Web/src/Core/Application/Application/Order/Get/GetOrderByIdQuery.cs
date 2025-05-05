using Application.Contracts.Order;
using Domain.Core.Results;
using MediatR;

namespace Application.Order.Get;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderResponse>>;