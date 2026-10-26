using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using KinemoPlatform.API.Movies.Application.Internal.CommandServices;
using KinemoPlatform.API.Movies.Application.Internal.DomainServices;
using KinemoPlatform.API.Movies.Application.Internal.QueryServices;
using KinemoPlatform.API.Movies.Domain.Repositories;
using KinemoPlatform.API.Movies.Domain.Services;
using KinemoPlatform.API.Movies.Infrastructure.External.FileStorage;
using KinemoPlatform.API.Movies.Infrastructure.Persistence.EFC.Repositories;

using KinemoPlatform.API.Scheduling.Application.Internal.CommandServices;
using KinemoPlatform.API.Scheduling.Application.Internal.QueryServices;
using KinemoPlatform.API.Scheduling.Domain.Repositories;
using KinemoPlatform.API.Scheduling.Domain.Services;
using KinemoPlatform.API.Scheduling.Infrastructure.Persistence.EFC.Repositories;

using KinemoPlatform.API.Analytics.Application.Internal.CommandServices;
using KinemoPlatform.API.Analytics.Application.Internal.QueryServices;
using KinemoPlatform.API.Analytics.Domain.Repositories;
using KinemoPlatform.API.Analytics.Domain.Services;
using KinemoPlatform.API.Analytics.Infrastructure.Persistence.EFC.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
        options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
    throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
    else if (builder.Environment.IsProduction())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
    }
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "KinemoPlatform.API",
            Version = "v1",
            Description = "Kinemo Platform API — 4D Cinema Experience Management",
            TermsOfService = new Uri("https://kinemo-platform.com/tos"),
            Contact = new OpenApiContact
            {
                Name = "Kinemo Studios",
                Email = "contact@kinemo.com"
            },
            License = new OpenApiLicense
            {
                Name = "Apache 2.0",
                Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
            }
        });

    options.EnableAnnotations();
});

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// TODO: IAM Bounded Context
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserCommandService, UserCommandService>();
// builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// TODO: Subscriptions Bounded Context (BC09)
// builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
// builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
// builder.Services.AddScoped<ISubscriptionPlanCommandService, SubscriptionPlanCommandService>();
// builder.Services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();
// builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
// builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

// Movies Bounded Context (BC01)
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieCommandService, MovieCommandService>();
builder.Services.AddScoped<IMovieQueryService, MovieQueryService>();
builder.Services.AddScoped<IMovieSensoryDomainService, MovieSensoryDomainService>();
builder.Services.AddScoped<ISensoryFileStorage, LocalSensoryFileStorage>();

// Scheduling Bounded Context (BC02)
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowCommandService, ShowCommandService>();
builder.Services.AddScoped<IShowQueryService, ShowQueryService>();

// TODO: Room & Resource Readiness Bounded Context (BC03)
// builder.Services.AddScoped<IRoomRepository, RoomRepository>();
// builder.Services.AddScoped<IRoomInspectionRepository, RoomInspectionRepository>();
// builder.Services.AddScoped<IRoomBlockRepository, RoomBlockRepository>();
// builder.Services.AddScoped<IRoomCommandService, RoomCommandService>();
// builder.Services.AddScoped<IRoomQueryService, RoomQueryService>();

// TODO: Ticketing Integration Bounded Context (BC04)
// builder.Services.AddScoped<ITicketingIntegrationRepository, TicketingIntegrationRepository>();
// builder.Services.AddScoped<ISyncLogRepository, SyncLogRepository>();
// builder.Services.AddScoped<IShowOccupancyRepository, ShowOccupancyRepository>();
// builder.Services.AddScoped<ITicketingCommandService, TicketingCommandService>();
// builder.Services.AddScoped<ITicketingQueryService, TicketingQueryService>();

// TODO: Seat Allocation & Control Bounded Context (BC05)
// builder.Services.AddScoped<ISeatAllocationRepository, SeatAllocationRepository>();
// builder.Services.AddScoped<IShowSeatRepository, ShowSeatRepository>();
// builder.Services.AddScoped<IManualInterventionRepository, ManualInterventionRepository>();
// builder.Services.AddScoped<ISeatAllocationCommandService, SeatAllocationCommandService>();
// builder.Services.AddScoped<ISeatAllocationQueryService, SeatAllocationQueryService>();

// TODO: 4D Execution & Synchronization Bounded Context (BC06)
// builder.Services.AddScoped<IShowExecutionRepository, ShowExecutionRepository>();
// builder.Services.AddScoped<ISensorySequenceRepository, SensorySequenceRepository>();
// builder.Services.AddScoped<ISynchronizationEventRepository, SynchronizationEventRepository>();
// builder.Services.AddScoped<IEmergencyEventRepository, EmergencyEventRepository>();
// builder.Services.AddScoped<IShowExecutionCommandService, ShowExecutionCommandService>();
// builder.Services.AddScoped<IShowExecutionQueryService, ShowExecutionQueryService>();

// TODO: Resource Testing & Maintenance Bounded Context (BC07)
// builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
// builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
// builder.Services.AddScoped<IMaintenanceOrderRepository, MaintenanceOrderRepository>();
// builder.Services.AddScoped<IEquipmentTestRepository, EquipmentTestRepository>();
// builder.Services.AddScoped<IMaintenanceCommandService, MaintenanceCommandService>();
// builder.Services.AddScoped<IMaintenanceQueryService, MaintenanceQueryService>();

// Analytics Bounded Context (BC08)
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IOperationalReportRepository, OperationalReportRepository>();
builder.Services.AddScoped<IDashboardCommandService, DashboardCommandService>();
builder.Services.AddScoped<IDashboardQueryService, DashboardQueryService>();
builder.Services.AddScoped<IOperationalReportCommandService, OperationalReportCommandService>();
builder.Services.AddScoped<IOperationalReportQueryService, OperationalReportQueryService>();

// Mediator Configuration
builder.Services.AddScoped(
    typeof(ICommandPipelineBehavior<>),
    typeof(LoggingCommandBehavior<>));

builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: new[] { typeof(Program) },
    configure: options =>
    {
        options.AddOpenCommandPipelineBehavior(
            typeof(LoggingCommandBehavior<>));
    });

var app = builder.Build();

// Database Initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}

// HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
