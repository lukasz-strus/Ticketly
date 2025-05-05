using Domain.Core.Results;
using MediatR;

namespace Application.Order.Status;

public sealed record CancelOrderCommand(
    Guid OrderId) : IRequest<Result>;