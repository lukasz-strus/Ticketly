using Application.Contracts.Common;
using Application.Contracts.Order;
using Domain.Core.Results;
using MediatR;

namespace Application.Order.Create;

public sealed record CreateOrderCommand(
    CreateOrderRequest Request) : IRequest<Result<EntityCreatedResponse>>;