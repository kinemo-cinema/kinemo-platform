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
[Tags("Subscription Plan")]
public class SubscriptionPlansController(ISubscriptionPlanQueryService queryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get All Plans")]
    [EndpointDescription("Retrieve all available subscription plans for pricing tables.")]
    [ProducesResponseType(typeof(IEnumerable<SubscriptionPlanResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPlans()
    {
        var plans = await queryService.Handle(new GetAllPlansQuery());
        return Ok(plans.Select(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity));
    }
}