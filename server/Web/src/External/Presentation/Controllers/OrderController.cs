using Application.Contracts.Common;
using Application.Contracts.Order;
using Application.Order.Create;
using Application.Order.Get;
using Application.Order.OrderItem.Add;
using Application.Order.OrderItem.Delete;
using Application.Order.OrderItem.Update;
using Application.Order.Status;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Infrastructure;

namespace Presentation.Controllers;

public sealed class OrderController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost(ApiRoutes.Order.Create)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateOrderCommand(value))
            .Bind(command => Mediator.Send(command))
            .Match(
                entityCreated => CreatedAtAction(nameof(GetOrderById), new { id = entityCreated.Id }, entityCreated),
                BadRequest);


    [HttpGet(ApiRoutes.Order.Get)]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrderById(Guid id) =>
        await Result.Success(new GetOrderByIdQuery(id))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    [HttpGet(ApiRoutes.Order.GetPending)]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPendingOrder() =>
        await Result.Success(new GetPendingOrderQuery())
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    [HttpPost(ApiRoutes.Order.CompleteOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteOrder(Guid id) =>
        await Result.Success(new CompleteOrderCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpPost(ApiRoutes.Order.CancelOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelOrder(Guid id) =>
        await Result.Success(new CancelOrderCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpPost(ApiRoutes.Order.OpenOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> OpenOrder(Guid id) =>
        await Result.Success(new OpenOrderCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpPost(ApiRoutes.Order.AddOrderItem)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddOrderItem(
        Guid orderId,
        AddOrderItemRequest request) =>
        await Result.Success(request)
            .Map(value => new AddOrderItemCommand(orderId, value))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpDelete(ApiRoutes.Order.DeleteOrderItem)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOrderItem(
        Guid orderId,
        Guid orderItemId) =>
        await Result.Success(new DeleteOrderItemCommand(orderId, orderItemId))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpPut(ApiRoutes.Order.UpdateOrderItem)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOrderItem(
        Guid orderId,
        Guid orderItemId,
        UpdateOrderItemRequest request) =>
        await Result.Success(request)
            .Map(value => new UpdateOrderItemCommand(orderId, orderItemId, value))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);
}