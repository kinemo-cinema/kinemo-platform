using System.Net.Mime;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Domain.Services;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Resources;
using KinemoPlatform.API.Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Subscriptions.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Subscription Endpoints.")]
public class SubscriptionsController(
    ISubscriptionCommandService commandService,
    ISubscriptionQueryService queryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create Subscription", Description = "Contract a new service plan for a cinema (US39).", OperationId = "CreateSubscription")]
    [SwaggerResponse(StatusCodes.Status201Created, "The subscription was created.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The plan does not exist.")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource)
    {
        try
        {
            var command = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
            var subscription = await commandService.Handle(command);
            if (subscription is null) return BadRequest();
            
            var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return CreatedAtAction(nameof(GetSubscriptionById), new { subscriptionId = subscription.Id }, subscriptionResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{subscriptionId:int}")]
    [SwaggerOperation(Summary = "Get Subscription by Id", OperationId = "GetSubscriptionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription was found.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The subscription was not found.")]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByIdQuery(subscriptionId));
        if (subscription is null) return NotFound();
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpGet("cinema/{cinemaId:int}")]
    [SwaggerOperation(Summary = "Get Subscription by Cinema Id", Description = "Check the current subscription status for a cinema (US41).", OperationId = "GetSubscriptionByCinemaId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The subscription was found.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No subscription found for this cinema.")]
    public async Task<IActionResult> GetSubscriptionByCinemaId([FromRoute] int cinemaId)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByCinemaIdQuery(cinemaId));
        if (subscription is null) return NotFound();
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpPost("{subscriptionId:int}/payments")]
    [SwaggerOperation(Summary = "Process Payment", Description = "Process a subscription payment (US40).", OperationId = "ProcessPayment")]
    [SwaggerResponse(StatusCodes.Status200OK, "The payment was processed.", typeof(SubscriptionResource))]
    public async Task<IActionResult> ProcessPayment([FromRoute] int subscriptionId, [FromBody] ProcessPaymentResource resource)
    {
        try
        {
            var command = ProcessPaymentCommandFromResourceAssembler.ToCommandFromResource(resource, subscriptionId);
            var subscription = await commandService.Handle(command);
            if (subscription is null) return NotFound();
            return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost("{subscriptionId:int}/upgrades")]
    [SwaggerOperation(Summary = "Change Plan", Description = "Upgrade or downgrade a subscription (US42).", OperationId = "ChangePlan")]
    [SwaggerResponse(StatusCodes.Status200OK, "The plan was changed.", typeof(SubscriptionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The new plan does not exist or is the same as the current one.")]
    public async Task<IActionResult> ChangePlan([FromRoute] int subscriptionId, [FromBody] ChangePlanResource resource)
    {
        try
        {
            var command = ChangePlanCommandFromResourceAssembler.ToCommandFromResource(resource, subscriptionId);
            var subscription = await commandService.Handle(command);
            if (subscription is null) return NotFound();
            return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}