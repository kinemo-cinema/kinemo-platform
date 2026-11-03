using KinemoPlatform.API.Analytics.Application.Errors;
using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KinemoPlatform.API.Analytics.Interfaces.REST.Transform;

public static class AnalyticsErrorAssembler
{
    public static ProblemDetails ToProblemDetails(
        EAnalyticsError error,
        IStringLocalizer<SharedResource> localizer)
    {
        var (status, key) = error switch
        {
            EAnalyticsError.InvalidPeriod =>
                (StatusCodes.Status400BadRequest, "Analytics.InvalidPeriod"),
            EAnalyticsError.DashboardNotFound =>
                (StatusCodes.Status404NotFound, "Analytics.DashboardNotFound"),
            EAnalyticsError.ReportNotFound =>
                (StatusCodes.Status404NotFound, "Analytics.ReportNotFound"),
            _ => (StatusCodes.Status500InternalServerError, "")
        };

        return new ProblemDetails
        {
            Status = status,
            Title = string.IsNullOrEmpty(key)
                ? localizer["UnexpectedServerError"].Value
                : localizer[$"{key}.Title"].Value,
            Detail = string.IsNullOrEmpty(key)
                ? localizer["UnexpectedErrorProcessingRequest"].Value
                : localizer[$"{key}.Detail"].Value
        };
    }
}
