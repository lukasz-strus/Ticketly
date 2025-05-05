using Application.Authentication.UserContext;
using Application.Contracts.Common;
using Application.Core.Abstractions.Data;
using Domain;
using Domain.Core.Results;
using Domain.OrderAggregate;
using Domain.UserAggregate;
using Domain.ValueObjects;
using MediatR;

namespace Application.Order.Create;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IUserContext userContext,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<EntityCreatedResponse>>
{
    public async Task<Result<EntityCreatedResponse>> Handle(CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        var orderResult = currentUser is not null
            ? await CreateOrderWithUser(
                currentUser.Id,
                cancellationToken)
            : CreateOrderWithoutUser(
                request.Request.FirstName,
                request.Request.LastName,
                request.Request.AddressStreet,
                request.Request.AddressBuilding,
                request.Request.AddressRoom,
                request.Request.AddressCode,
                request.Request.AddressPost);

        if (orderResult.IsFailure)
            return Result.Failure<EntityCreatedResponse>(orderResult.Error);

        var order = orderResult.Value();

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new EntityCreatedResponse(order.Id.Value));
    }

    private async Task<Result<Domain.OrderAggregate.Order>> CreateOrderWithUser(
        string userId,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetPendingByUserIdAsync(userId, cancellationToken);
        if (order is not null)
            return Result.Failure<Domain.OrderAggregate.Order>(OrderErrors.Create.OrderAlreadyExists);

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        return user is null
            ? Result.Failure<Domain.OrderAggregate.Order>(Errors.General.EntityNotFound)
            : Domain.OrderAggregate.Order.Create(user);
    }

    private static Result<Domain.OrderAggregate.Order> CreateOrderWithoutUser(
        string? firstName,
        string? lastName,
        string? street,
        string? building,
        string? room,
        string? code,
        string? post)
    {
        if (string.IsNullOrEmpty(firstName))
            return Result.Failure<Domain.OrderAggregate.Order>(Errors.ValueObject.FirstNameIsRequired);

        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
            return Result.Failure<Domain.OrderAggregate.Order>(firstNameResult.Error);

        if (string.IsNullOrEmpty(lastName))
            return Result.Failure<Domain.OrderAggregate.Order>(Errors.ValueObject.LastNameIsRequired);

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
            return Result.Failure<Domain.OrderAggregate.Order>(lastNameResult.Error);

        if (string.IsNullOrEmpty(street) ||
            string.IsNullOrEmpty(building) ||
            string.IsNullOrEmpty(code) ||
            string.IsNullOrEmpty(post))
            return Result.Failure<Domain.OrderAggregate.Order>(Errors.ValueObject.AddressIsRequired);

        var addressResult =
            Address.Create(street, building, room, code, post);
        if (addressResult.IsFailure)
            return Result.Failure<Domain.OrderAggregate.Order>(addressResult.Error);

        return Domain.OrderAggregate.Order.Create(
            firstNameResult.Value(),
            lastNameResult.Value(),
            addressResult.Value());
    }
}