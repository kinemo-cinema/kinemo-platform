using System.Net.Mime;
using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Analytics.Application.Services;
using KinemoPlatform.API.Analytics.Domain.Model.Queries;
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
    [HttpPost("dashboards")]
    [EndpointSummary("Generate Dashboard")]
    [EndpointDescription("Generate a new operational dashboard for a specific period.")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateDashboard(
        [FromBody] GenerateDashboardResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            GenerateDashboardCommandFromResourceAssembler.ToCommandFromResource(resource);

        var result = await dashboardCommandService.Handle(
            command,
            cancellationToken);

        return result.Fold<IActionResult>(
            dashboard =>
            {
                var dashboardResource =
                    DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard);

                return CreatedAtAction(
                    nameof(GetDashboardById),
                    new { dashboardId = dashboard.Id },
                    dashboardResource);
            },
            MapAnalyticsError);
    }

    [HttpGet("dashboards/{dashboardId:int}")]
    [EndpointSummary("Get Dashboard by Id")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDashboardById(
        [FromRoute] int dashboardId,
        CancellationToken cancellationToken)
    {
        var dashboard = await dashboardQueryService.Handle(
            new GetDashboardByIdQuery(dashboardId),
            cancellationToken);

        if (dashboard is null)
            return NotFound();

        return Ok(
            DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard));
    }

    [HttpGet("dashboards/latest")]
    [EndpointSummary("Get Latest Dashboard")]
    [ProducesResponseType(typeof(DashboardResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLatestDashboard(
        CancellationToken cancellationToken)
    {
        var dashboard = await dashboardQueryService.Handle(
            new GetLatestDashboardQuery(),
            cancellationToken);

        if (dashboard is null)
            return NotFound();

        return Ok(
            DashboardResourceFromEntityAssembler.ToResourceFromEntity(dashboard));
    }

    [HttpPost("reports")]
    [EndpointSummary("Generate Report")]
    [EndpointDescription("Generate an operational report.")]
    [ProducesResponseType(typeof(OperationalReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateReport(
        [FromBody] GenerateReportResource resource,
        CancellationToken cancellationToken)
    {
        var command =
            GenerateReportCommandFromResourceAssembler.ToCommandFromResource(resource);

        var result = await reportCommandService.Handle(
            command,
            cancellationToken);

        return result.Fold<IActionResult>(
            report =>
            {
                var reportResource =
                    OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report);

                return CreatedAtAction(
                    nameof(GetReportById),
                    new { reportId = report.Id },
                    reportResource);
            },
            MapAnalyticsError);
    }

    [HttpGet("reports/{reportId:int}")]
    [EndpointSummary("Get Report by Id")]
    [ProducesResponseType(typeof(OperationalReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportById(
        [FromRoute] int reportId,
        CancellationToken cancellationToken)
    {
        var report = await reportQueryService.Handle(
            new GetReportByIdQuery(reportId),
            cancellationToken);

        if (report is null)
            return NotFound();

        return Ok(
            OperationalReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }

    private IActionResult MapAnalyticsError(EAnalyticsError error)
    {
        var problem = AnalyticsErrorAssembler.ToProblemDetails(error);

        return new ObjectResult(problem)
        {
            StatusCode = problem.Status,
            ContentTypes = { "application/problem+json" }
        };
    }
}
