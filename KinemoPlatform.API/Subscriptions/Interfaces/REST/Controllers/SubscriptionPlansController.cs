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
[SwaggerTag("Available Subscription Plan Endpoints.")]
public class SubscriptionPlansController(ISubscriptionPlanQueryService queryService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Get All Plans", Description = "Retrieve all available subscription plans for pricing tables.", OperationId = "GetAllPlans")]
    [SwaggerResponse(StatusCodes.Status200OK, "The plans were found.", typeof(IEnumerable<SubscriptionPlanResource>))]
    public async Task<IActionResult> GetAllPlans()
    {
        var plans = await queryService.Handle(new GetAllPlansQuery());
        return Ok(plans.Select(SubscriptionPlanResourceFromEntityAssembler.ToResourceFromEntity));
    }
}