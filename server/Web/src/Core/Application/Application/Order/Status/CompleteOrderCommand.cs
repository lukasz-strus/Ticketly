using Domain.Core.Results;
using MediatR;

namespace Application.Order.Status;

public sealed record CompleteOrderCommand(
    Guid OrderId) : IRequest<Result>;