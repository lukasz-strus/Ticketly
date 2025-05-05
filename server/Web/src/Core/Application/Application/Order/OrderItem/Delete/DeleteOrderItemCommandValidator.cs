using Application.Core.Utilities;
using Domain;
using FluentValidation;

namespace Application.Order.OrderItem.Delete;

internal sealed class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
{
    public DeleteOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);

        RuleFor(x => x.OrderItemId)
            .NotEmpty().WithError(Errors.ValueObject.IdIsRequired);
    }
}