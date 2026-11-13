using KinemoPlatform.API.Resources;
using Microsoft.Extensions.Localization;

using System.Net.Mime;
using KinemoPlatform.API.Subscriptions.Application.Errors;
using KinemoPlatform.API.Subscriptions.Application.Services;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Subscription")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var items = await queryService.Handle(new GetAllSubscriptionsQuery(), cancellationToken);
        return Ok(items.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("Create Subscription")]
    [EndpointDescription("Contract a new service plan for a cinema (US39).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);

        var result = await commandService.Handle(command, cancellationToken);

        return result.Fold<IActionResult>(
            subscription =>
            {
                var subscriptionResource =
                    SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);

                return CreatedAtAction(
                    nameof(GetSubscriptionById),
                    new { subscriptionId = subscription.Id },
                    subscriptionResource);
            },
            MapSubscriptionError);
    }

    [HttpGet("{subscriptionId:int}")]
    [EndpointSummary("Get Subscription by Id")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById(
        [FromRoute] int subscriptionId,
        CancellationToken cancellationToken)
    {
        var subscription = await queryService.Handle(
            new GetSubscriptionByIdQuery(subscriptionId),
            cancellationToken);

        if (subscription is null)
            return NotFound();

        return Ok(
            SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpGet("cinema/{cinemaId:int}")]
    [EndpointSummary("Get Subscription by Cinema Id")]
    [EndpointDescription("Check the current subscription status for a cinema (US41).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionByCinemaId(
        [FromRoute] int cinemaId,
        CancellationToken cancellationToken)
    {
        var subscription = await queryService.Handle(
            new GetSubscriptionByCinemaIdQuery(cinemaId),
            cancellationToken);

        if (subscription is null)
            return NotFound();

        return Ok(
            SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpPost("{subscriptionId:int}/payments")]
    [EndpointSummary("Process Payment")]
    [EndpointDescription("Process a subscription payment (US40).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessPayment(
        [FromRoute] int subscriptionId,
        [FromBody] ProcessPaymentResource resource,
        CancellationToken cancellationToken)
    {
        var command = ProcessPaymentCommandFromResourceAssembler.ToCommandFromResource(
            resource, subscriptionId);

        var result = await commandService.Handle(command, cancellationToken);

        return result.Fold<IActionResult>(
            subscription => Ok(
                SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)),
            MapSubscriptionError);
    }

    [HttpPost("{subscriptionId:int}/upgrades")]
    [EndpointSummary("Change Plan")]
    [EndpointDescription("Upgrade or downgrade a subscription (US42).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangePlan(
        [FromRoute] int subscriptionId,
        [FromBody] ChangePlanResource resource,
        CancellationToken cancellationToken)
    {
        var command = ChangePlanCommandFromResourceAssembler.ToCommandFromResource(
            resource, subscriptionId);

        var result = await commandService.Handle(command, cancellationToken);

        return result.Fold<IActionResult>(
            subscription => Ok(
                SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription)),
            MapSubscriptionError);
    }

    private IActionResult MapSubscriptionError(ESubscriptionError error)
    {
        var problem = SubscriptionErrorAssembler.ToProblemDetails(error, HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>());

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status,
            ContentTypes = { "application/problem+json" }
        };
    }
}
