using Application.Core.Utilities;
using Domain;
using Domain.EventAggregate;
using FluentValidation;

namespace Application.Event.Event.UpdateTicketPool;

internal sealed class UpdateTicketPoolCommandValidator : AbstractValidator<UpdateTicketPoolCommand>
{
    public UpdateTicketPoolCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.TicketPoolId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.Request.AvailableTickets)
            .GreaterThan(0u).WithError(EventErrors.TicketPools.AvailableTicketsCountMustBeGreaterThan0);

        RuleFor(x => x.Request.Price)
            .GreaterThan(0m).WithError(EventErrors.TicketPools.PriceMustBeGreaterThan0);

        RuleFor(x => x.Request.StartDate)
            .LessThan(x => x.Request.EndDate)
            .WithError(EventErrors.TicketPools.TicketPoolStartSaleDateMustBeLowerThanEndSaleDate);

        RuleFor(x => x.Request.EndDate)
            .GreaterThan(x => x.Request.StartDate)
            .WithError(EventErrors.TicketPools.TicketPoolEndSaleDateMustBeLessThanEventDate);
    }
}