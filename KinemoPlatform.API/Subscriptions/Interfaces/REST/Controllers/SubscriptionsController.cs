using System.Net.Mime;
using KinemoPlatform.API.Subscriptions.Domain.Model.Queries;
using KinemoPlatform.API.Subscriptions.Domain.Services;
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
    [HttpPost]
    [EndpointSummary("Create Subscription")]
    [EndpointDescription("Contract a new service plan for a cinema (US39).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [EndpointSummary("Get Subscription by Id")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByIdQuery(subscriptionId));
        if (subscription is null) return NotFound();
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpGet("cinema/{cinemaId:int}")]
    [EndpointSummary("Get Subscription by Cinema Id")]
    [EndpointDescription("Check the current subscription status for a cinema (US41).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubscriptionByCinemaId([FromRoute] int cinemaId)
    {
        var subscription = await queryService.Handle(new GetSubscriptionByCinemaIdQuery(cinemaId));
        if (subscription is null) return NotFound();
        return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
    }

    [HttpPost("{subscriptionId:int}/payments")]
    [EndpointSummary("Process Payment")]
    [EndpointDescription("Process a subscription payment (US40).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
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
    [EndpointSummary("Change Plan")]
    [EndpointDescription("Upgrade or downgrade a subscription (US42).")]
    [ProducesResponseType(typeof(SubscriptionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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