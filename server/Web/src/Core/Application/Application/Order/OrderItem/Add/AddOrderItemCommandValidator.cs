using Application.Core.Utilities;
using Domain;
using FluentValidation;

namespace Application.Order.OrderItem.Add;

internal sealed class AddOrderItemCommandValidator : AbstractValidator<AddOrderItemCommand>
{
    public AddOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.Request.EventId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.Request.TicketPoolId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);
    }
}