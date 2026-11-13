using KinemoPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;

namespace KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Documentation;

/// <summary>
///     Localizes the generated OpenAPI operations using the <c>SharedResource</c> resource files.
/// </summary>
/// <remarks>
///     <para>
///         The English summary and description live next to each endpoint
///         (<c>[EndpointSummary]</c> / <c>[EndpointDescription]</c>). A resource key overrides them
///         for the request culture, so translating an endpoint only requires adding keys:
///     </para>
///     <list type="bullet">
///         <item><c>&lt;Controller&gt;.&lt;Action&gt;.Summary</c></item>
///         <item><c>&lt;Controller&gt;.&lt;Action&gt;.Description</c></item>
///         <item><c>&lt;Controller&gt;.&lt;Action&gt;.Response&lt;StatusCode&gt;</c> (e.g. <c>Movies.GetMovieById.Response404</c>)</item>
///     </list>
///     <para>
///         Keys that are not found are ignored, so partially translated endpoints keep their default text.
///     </para>
/// </remarks>
public static class LocalizedOpenApiTransformer
{
    /// <summary>
    ///     Sets the operation id and localizes the summary, description and response descriptions of an operation.
    /// </summary>
    /// <param name="operation">The OpenAPI operation being transformed.</param>
    /// <param name="context">The transformer context, used to resolve the localizer and the action descriptor.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A completed task.</returns>
    public static Task TransformOperationAsync(
        OpenApiOperation operation,
        OpenApiOperationTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action)
            return Task.CompletedTask;

        var localizer = context.ApplicationServices.GetRequiredService<IStringLocalizer<SharedResource>>();
        var prefix = $"{action.ControllerName}.{action.ActionName}";

        // Action names are unique across the API, so they make stable operation ids for client generators.
        operation.OperationId = action.ActionName;

        var summary = localizer[$"{prefix}.Summary"];
        if (!summary.ResourceNotFound) operation.Summary = summary.Value;

        var description = localizer[$"{prefix}.Description"];
        if (!description.ResourceNotFound) operation.Description = description.Value;

        if (operation.Responses is null) return Task.CompletedTask;

        foreach (var statusCode in operation.Responses.Keys.ToArray())
        {
            var text = localizer[$"{prefix}.Response{statusCode}"];
            if (text.ResourceNotFound) continue;

            if (operation.Responses.TryGetValue(statusCode, out var response) &&
                response is OpenApiResponse openApiResponse)
                openApiResponse.Description = text.Value;
        }

        return Task.CompletedTask;
    }
}