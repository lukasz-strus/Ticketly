using Domain.Core.Results;
using MediatR;

namespace Application.Event.Event.Delete;

public sealed record DeleteEventCommand(
    Guid Id) : IRequest<Result>;
