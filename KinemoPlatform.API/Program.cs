using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

// Shared Bounded Context
using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Documentation;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Resources;

// BC01: Movies
using KinemoPlatform.API.Movies.Application.Internal.CommandServices;
using KinemoPlatform.API.Movies.Application.Internal.DomainServices;
using KinemoPlatform.API.Movies.Application.Internal.QueryServices;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Movies.Domain.Services;
using KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;
using KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Repositories;

// BC02: Scheduling
using KinemoPlatform.API.Scheduling.Application.Internal.CommandServices;
using KinemoPlatform.API.Scheduling.Application.Internal.QueryServices;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Scheduling.Domain.Services;
using KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Repositories;

// BC03: Rooms
using KinemoPlatform.API.Rooms.Application.Internal.CommandServices;
using KinemoPlatform.API.Rooms.Application.Internal.QueryServices;
using KinemoPlatform.API.Rooms.Domain.Repositories;
using KinemoPlatform.API.Rooms.Infrastructure.Persistence.EFC.Repositories;

// BC04: Ticketing
using KinemoPlatform.API.Ticketing.Application.Internal.CommandServices;
using KinemoPlatform.API.Ticketing.Application.Internal.QueryServices;
using KinemoPlatform.API.Ticketing.Domain.ExternalServices;
using KinemoPlatform.API.Ticketing.Domain.Repositories;
using KinemoPlatform.API.Ticketing.Domain.Services;
using KinemoPlatform.API.Ticketing.Infrastructure.External;
using KinemoPlatform.API.Ticketing.Infrastructure.Persistence.EFC.Repositories;

// BC05: Seats
using KinemoPlatform.API.Seats.Application.Internal.CommandServices;
using KinemoPlatform.API.Seats.Application.Internal.QueryServices;
using KinemoPlatform.API.Seats.Domain.Repositories;
using KinemoPlatform.API.Seats.Domain.Services;
using KinemoPlatform.API.Seats.Infrastructure.Persistence.EFC.Repositories;


// BC06: Execution
using KinemoPlatform.API.Execution.Application.Internal.CommandServices;
using KinemoPlatform.API.Execution.Application.Internal.QueryServices;
using KinemoPlatform.API.Execution.Domain.Repositories;
using KinemoPlatform.API.Execution.Domain.Services;
using KinemoPlatform.API.Execution.Infrastructure.Persistence.EFC.Repositories;

// BC07: Maintenance
using KinemoPlatform.API.Maintenance.Application.Internal.CommandServices;
using KinemoPlatform.API.Maintenance.Application.Internal.QueryServices;
using KinemoPlatform.API.Maintenance.Domain.Repositories;
using KinemoPlatform.API.Maintenance.Domain.Services;
using KinemoPlatform.API.Maintenance.Infrastructure.Persistence.EFC.Repositories;

// BC08: Analytics
using KinemoPlatform.API.Analytics.Application.Internal.CommandServices;
using KinemoPlatform.API.Analytics.Application.Internal.QueryServices;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Repositories;
using KinemoPlatform.API.Movies.Application.Services;
using KinemoPlatform.API.Rooms.Application.Services;

// BC09: Subscriptions
using KinemoPlatform.API.Subscriptions.Application.Internal.CommandServices;
using KinemoPlatform.API.Subscriptions.Application.Internal.QueryServices;
using KinemoPlatform.API.Subscriptions.Domain.Repositories;
using KinemoPlatform.API.Subscriptions.Domain.Services;
using KinemoPlatform.API.Subscriptions.Infrastructure.Persistence.EFC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Localization: messages come from Resources/SharedResource*.resx
builder.Services.AddLocalization();

// Add services to the container.
builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    })
    // Validation attributes (e.g. [Required(ErrorMessage = "RequiredTitle")]) resolve their message as a resource key.
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(SharedResource)))
    // Invalid request bodies produce a localized RFC 7807 (application/problem+json) validation response.
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var localizer = context.HttpContext.RequestServices
                .GetRequiredService<IStringLocalizer<SharedResource>>();
            var problemDetailsFactory = context.HttpContext.RequestServices
                .GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState,
                statusCode: StatusCodes.Status400BadRequest,
                title: localizer["InvalidModelStateTitle"].Value);

            var result = new BadRequestObjectResult(problemDetails);
            result.ContentTypes.Add("application/problem+json");
            return result;
        };
    });

// RFC 7807 ProblemDetails for unhandled exceptions (500) and error status codes.
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        if (context.ProblemDetails.Status is null or >= 500)
        {
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();
            context.ProblemDetails.Title ??= localizer["UnexpectedServerError"].Value;
            context.ProblemDetails.Detail ??= localizer["UnexpectedErrorProcessingRequest"].Value;
        }
    };
});

// Generate the OpenAPI document with ASP.NET Core's built-in support (served at /openapi/v1.json).
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer(LocalizedOpenApiTransformer.TransformOperationAsync);
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "KinemoPlatform.API",
            Version = "v1",
            Description = "Kinemo Platform API — 4D Cinema Experience Management",
            TermsOfService = new Uri("https://kinemo-platform.com/tos"),
            Contact = new OpenApiContact { Name = "Kinemo Studios", Email = "contact@kinemo.com" },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        };
        return Task.CompletedTask;
    });
});

// Database: the connection string may contain %ENVIRONMENT_VARIABLES% (see appsettings.Production.json).
var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionStringTemplate))
    throw new InvalidOperationException("Database connection string is not set in the configuration.");

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    // The provider is configured for every environment; only diagnostics differ.
    // EF Core logs go through the application's ILogger pipeline (see the "Logging" section of appsettings).
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});


// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// BC01: Movies
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieCommandService, MovieCommandService>();
builder.Services.AddScoped<IMovieQueryService, MovieQueryService>();
builder.Services.AddScoped<IMovieSensoryDomainService, MovieSensoryDomainService>();
builder.Services.AddScoped<ISensoryFileStorage, LocalSensoryFileStorage>();

// BC02: Scheduling
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowCommandService, ShowCommandService>();
builder.Services.AddScoped<IShowQueryService, ShowQueryService>();

// BC03: Rooms
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomCommandService, RoomCommandService>();
builder.Services.AddScoped<IRoomQueryService, RoomQueryService>();

// BC04: Ticketing
builder.Services.AddScoped<ITicketingRepository, TicketingRepository>();
builder.Services.AddScoped<ITicketingCommandService, TicketingCommandService>();
builder.Services.AddScoped<ITicketingQueryService, TicketingQueryService>();
builder.Services.AddScoped<IExternalTicketingClient, MockExternalTicketingClient>();

// BC05: Seats
builder.Services.AddScoped<ISeatAllocationRepository, SeatAllocationRepository>();
builder.Services.AddScoped<ISeatAllocationCommandService, SeatAllocationCommandService>();
builder.Services.AddScoped<ISeatAllocationQueryService, SeatAllocationQueryService>();

// BC06: Execution
builder.Services.AddScoped<IExecutionRepository, ExecutionRepository>();
builder.Services.AddScoped<IShowExecutionCommandService, ShowExecutionCommandService>();
builder.Services.AddScoped<IShowExecutionQueryService, ShowExecutionQueryService>();

// BC07: Maintenance
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IMaintenanceOrderRepository, MaintenanceOrderRepository>();
builder.Services.AddScoped<IEquipmentCommandService, EquipmentCommandService>();
builder.Services.AddScoped<IEquipmentQueryService, EquipmentQueryService>();
builder.Services.AddScoped<IMaintenanceOrderCommandService, MaintenanceOrderCommandService>();
builder.Services.AddScoped<IMaintenanceOrderQueryService, MaintenanceOrderQueryService>();

// BC08: Analytics
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IOperationalReportRepository, OperationalReportRepository>();
builder.Services.AddScoped<IDashboardCommandService, DashboardCommandService>();
builder.Services.AddScoped<IDashboardQueryService, DashboardQueryService>();
builder.Services.AddScoped<IOperationalReportCommandService, OperationalReportCommandService>();
builder.Services.AddScoped<IOperationalReportQueryService, OperationalReportQueryService>();

// BC09: Subscriptions
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();
builder.Services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();

// Mediator Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));

builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: new[] { typeof(Program) },
    configure: options =>
    {
        options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>));
    });

var app = builder.Build();

// Apply pending migrations on startup (safe to call even when the schema is already up to date)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// HTTP Request Pipeline

// Turns unhandled exceptions into ProblemDetails responses instead of leaking stack traces.
app.UseExceptionHandler();

// Localization must run before OpenAPI so the generated documentation follows the request culture.
// "en" is the default; requests for regional variants (es-PE, en-GB, ...) fall back to their parent culture.
string[] supportedCultures = ["en", "en-US", "es", "es-419"];
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
localizationOptions.ApplyCurrentCultureToResponseHeaders = true;
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseAuthorization();

// OpenAPI document (/openapi/v1.json) and Scalar API reference (/scalar).
// Available in every environment so the deployed API can be explored; wrap these two calls in
// 'if (app.Environment.IsDevelopment())' to restrict them to local development.
app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

app.Run();