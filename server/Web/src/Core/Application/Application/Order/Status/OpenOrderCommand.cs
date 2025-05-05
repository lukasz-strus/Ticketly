using Domain.Core.Results;
using MediatR;

namespace Application.Order.Status;

public sealed record OpenOrderCommand(
    Guid OrderId) : IRequest<Result>;