using Application.Contracts.Common;
using Application.Contracts.Event;
using Application.Event.Category.Create;
using Application.Event.Category.Delete;
using Application.Event.Category.Get;
using Application.Event.Category.GetAll;
using Application.Event.Event.AddTicketPool;
using Application.Event.Event.Create;
using Application.Event.Event.Delete;
using Application.Event.Event.Get;
using Application.Event.Event.GetAll;
using Application.Event.Event.RemoveTicketPool;
using Application.Event.Event.Update;
using Application.Event.Event.UpdateTicketPool;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using Domain.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using Presentation.Infrastructure;

namespace Presentation.Controllers;

public sealed class EventController(IMediator mediator) : ApiController(mediator)
{
    #region Category

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Category.Create)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateCategoryCommand(value))
            .Bind(command => Mediator.Send(command))
            .Match(
                entityCreated => CreatedAtAction(nameof(GetCategoryById), new { id = entityCreated.Id }, entityCreated),
                BadRequest);

    [HttpGet(ApiRoutes.Category.GetById)]
    [ProducesResponseType(typeof(CategoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(Guid id) =>
        await Result.Success(new GetCategoryByIdQuery(id))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    [HttpGet(ApiRoutes.Category.Get)]
    [ProducesResponseType(typeof(CategoryListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCategories() =>
        await Result.Success(new GetAllCategoriesQuery())
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpDelete(ApiRoutes.Category.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCategory(Guid id) =>
        await Result.Success(new DeleteCategoryCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    #endregion

    #region Event

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Event.Create)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEvent(CreateEventRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateEventCommand(value))
            .Bind(command => Mediator.Send(command))
            .Match(
                entityCreated => CreatedAtAction(nameof(GetEventById), new { id = entityCreated.Id }, entityCreated),
                BadRequest);

    [HttpGet(ApiRoutes.Event.GetById)]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEventById(Guid id) =>
        await Result.Success(new GetEventByIdQuery(id))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);


    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPut(ApiRoutes.Event.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEvent(Guid id, UpdateEventRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new UpdateEventCommand(id, value))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);


    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpDelete(ApiRoutes.Event.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteEvent(Guid id) =>
        await Result.Success(new DeleteEventCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [HttpGet(ApiRoutes.Event.Get)]
    [ProducesResponseType(typeof(EventListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllEvents() =>
        await Result.Success(new GetAllEventsQuery(null))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    [HttpGet(ApiRoutes.Event.GetByCategoryId)]
    [ProducesResponseType(typeof(EventListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllCategoryEvents(
        Guid id) =>
        await Result.Success(new GetAllEventsQuery(id))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, NotFound);

    #endregion

    #region TicketPools

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPost(ApiRoutes.Event.AddTicketPool)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTicketPool(
        Guid eventId,
        AddTicketPoolRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new AddTicketPoolCommand(eventId, value))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpPut(ApiRoutes.Event.UpdateTicketPool)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTicketPool(
        Guid eventId,
        Guid ticketPoolId,
        UpdateTicketPoolRequest request) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new UpdateTicketPoolCommand(eventId, ticketPoolId, value))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    [Authorize(Roles = UserRoleNames.Admin)]
    [HttpDelete(ApiRoutes.Event.DeleteTicketPool)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTicketPool(
        Guid eventId,
        Guid ticketPoolId) =>
        await Result.Success(new RemoveTicketPoolCommand(eventId, ticketPoolId))
            .Bind(command => Mediator.Send(command))
            .Match(NoContent, BadRequest);

    #endregion
}