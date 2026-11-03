using KinemoPlatform.API.Analytics.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;

public static class AnalyticsErrorAssembler
{
    public static ProblemDetails ToProblemDetails(EAnalyticsError error)
    {
        return error switch
        {
            EAnalyticsError.InvalidPeriod => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid period",
                Detail = "The start date must be earlier than the end date."
            },

            EAnalyticsError.DashboardNotFound => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Dashboard not found",
                Detail = "The requested dashboard does not exist."
            },

            EAnalyticsError.ReportNotFound => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Report not found",
                Detail = "The requested operational report does not exist."
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unexpected analytics error"
            }
        };
    }
}