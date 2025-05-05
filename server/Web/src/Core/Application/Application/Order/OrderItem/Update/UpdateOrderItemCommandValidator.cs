using Application.Core.Utilities;
using Domain;
using FluentValidation;

namespace Application.Order.OrderItem.Update;

internal sealed class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
{
    public UpdateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.OrderItemId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);
    }
}