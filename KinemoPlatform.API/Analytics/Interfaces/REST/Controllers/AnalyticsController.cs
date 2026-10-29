using System.Net.Mime;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Analytics.Interfaces.REST.Resources;
using KinemoPlatform.API.Analytics.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Analytics")]
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
    [EndpointSummary("Generate Dashboard")]
    [EndpointDescription("Generate a new operational dashboard for a specific period.")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> GenerateDashboard([FromBody] GenerateDashboardResource resource)
    {
        var command = GenerateDashboardCommandFromResourceAssembler.ToCommandFromResource(resource);
        var dashboard = await dashboardCommandService.Handle(command);
        if (dashboard is null) return BadRequest();
        
        var dashboardResource = DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard);
        return CreatedAtAction(nameof(GetDashboardById), new { dashboardId = dashboard.Id }, dashboardResource);
    }

    [HttpGet("dashboards/{dashboardId:int}")]
    [EndpointSummary("Get Dashboard by Id")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDashboardById([FromRoute] int dashboardId)
    {
        var dashboard = await dashboardQueryService.Handle(new GetDashboardByIdQuery(dashboardId));
        if (dashboard is null) return NotFound();
        return Ok(DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard));
    }

    [HttpGet("dashboards/latest")]
    [EndpointSummary("Get Latest Dashboard")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [EndpointSummary("Generate Report")]
    [EndpointDescription("Generate an operational report.")]
    [ProducesResponseType(typeof(OperationalReportResource), StatusCodes.Status201Created)]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateReportResource resource)
    {
        var command = GenerateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var report = await reportCommandService.Handle(command);
        if (report is null) return BadRequest();
        
        var reportResource = OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report);
        return CreatedAtAction(nameof(GetReportById), new { reportId = report.Id }, reportResource);
    }

    [HttpGet("reports/{reportId:int}")]
    [EndpointSummary("Get Report by Id")]
    [ProducesResponseType(typeof(OperationalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportById([FromRoute] int reportId)
    {
        var report = await reportQueryService.Handle(new GetReportByIdQuery(reportId));
        if (report is null) return NotFound();
        return Ok(OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }
}