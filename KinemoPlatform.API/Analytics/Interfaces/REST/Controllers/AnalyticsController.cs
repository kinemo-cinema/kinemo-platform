using System.Net.Mime;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
using KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Analytics Endpoints.")]
public class AnalyticsController(
    IDashboardCommandService dashboardCommandService,
    IDashboardQueryService dashboardQueryService,
    IOperationalReportCommandService reportCommandService,
    IOperationalReportQueryService reportQueryService) : ControllerBase
{
    // ---------------------------------------------------------------------
    // Dashboard Endpoints
    // ---------------------------------------------------------------------
    [HttpPost("dashboards")]
    [SwaggerOperation(Summary = "Generate Dashboard", Description = "Generate a new operational dashboard for a specific period.", OperationId = "GenerateDashboard")]
    [SwaggerResponse(StatusCodes.Status201Created, "The dashboard was generated.", typeof(DashboardResource))]
    public async Task<IActionResult> GenerateDashboard([FromBody] GenerateDashboardResource resource)
    {
        var command = GenerateDashboardCommandFromResourceAssembler.ToCommandFromResource(resource);
        var dashboard = await dashboardCommandService.Handle(command);
        if (dashboard is null) return BadRequest();
        
        var dashboardResource = DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard);
        return CreatedAtAction(nameof(GetDashboardById), new { dashboardId = dashboard.Id }, dashboardResource);
    }

    [HttpGet("dashboards/{dashboardId:int}")]
    [SwaggerOperation(Summary = "Get Dashboard by Id", OperationId = "GetDashboardById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The dashboard was found.", typeof(DashboardResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The dashboard was not found.")]
    public async Task<IActionResult> GetDashboardById([FromRoute] int dashboardId)
    {
        var dashboard = await dashboardQueryService.Handle(new GetDashboardByIdQuery(dashboardId));
        if (dashboard is null) return NotFound();
        return Ok(DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard));
    }

    [HttpGet("dashboards/latest")]
    [SwaggerOperation(Summary = "Get Latest Dashboard", OperationId = "GetLatestDashboard")]
    [SwaggerResponse(StatusCodes.Status200OK, "The latest dashboard was found.", typeof(DashboardResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No dashboards exist.")]
    public async Task<IActionResult> GetLatestDashboard()
    {
        var dashboard = await dashboardQueryService.Handle(new GetLatestDashboardQuery());
        if (dashboard is null) return NotFound();
        return Ok(DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard));
    }

    // ---------------------------------------------------------------------
    // Report Endpoints
    // ---------------------------------------------------------------------
    [HttpPost("reports")]
    [SwaggerOperation(Summary = "Generate Report", Description = "Generate an operational report.", OperationId = "GenerateReport")]
    [SwaggerResponse(StatusCodes.Status201Created, "The report was generated.", typeof(OperationalReportResource))]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateReportResource resource)
    {
        var command = GenerateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var report = await reportCommandService.Handle(command);
        if (report is null) return BadRequest();
        
        var reportResource = OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return CreatedAtAction(nameof(GetReportById), new { reportId = report.Id }, reportResource);
    }

    [HttpGet("reports/{reportId:int}")]
    [SwaggerOperation(Summary = "Get Report by Id", OperationId = "GetReportById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The report was found.", typeof(OperationalReportResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The report was not found.")]
    public async Task<IActionResult> GetReportById([FromRoute] int reportId)
    {
        var report = await reportQueryService.Handle(new GetReportByIdQuery(reportId));
        if (report is null) return NotFound();
        return Ok(OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }
}