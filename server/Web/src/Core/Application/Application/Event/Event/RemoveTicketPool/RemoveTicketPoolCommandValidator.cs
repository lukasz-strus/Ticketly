using Application.Core.Utilities;
using Domain;
using FluentValidation;

namespace Application.Event.Event.RemoveTicketPool;

internal sealed class RemoveTicketPoolCommandValidator : AbstractValidator<RemoveTicketPoolCommand>
{
    public RemoveTicketPoolCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.TicketPoolId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);
    }
}