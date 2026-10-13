using KinemoPlatform.API.Shared.Domain.Repositories;
using KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    else if (builder.Environment.IsProduction())
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
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

// TODO: Uncomment as you build your IAM Bounded Context
// builder.Services.AddScoped<IUserRepository, UserRepository>();
// builder.Services.AddScoped<IUserCommandService, UserCommandService>();
// builder.Services.AddScoped<IUserQueryService, UserQueryService>();

// TODO: Uncomment as you build your Subscription & Service Management Bounded Context (BC09)
// builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
// builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
// builder.Services.AddScoped<ISubscriptionPlanCommandService, SubscriptionPlanCommandService>();
// builder.Services.AddScoped<ISubscriptionPlanQueryService, SubscriptionPlanQueryService>();
// builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
// builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

// TODO: Uncomment as you build your Movie & Sensory Content Management Bounded Context (BC01)
// builder.Services.AddScoped<IMovieRepository, MovieRepository>();
// builder.Services.AddScoped<ISensoryContentRepository, SensoryContentRepository>();
// builder.Services.AddScoped<IMovieCommandService, MovieCommandService>();
// builder.Services.AddScoped<IMovieQueryService, MovieQueryService>();
// builder.Services.AddScoped<ISensoryContentCommandService, SensoryContentCommandService>();
// builder.Services.AddScoped<ISensoryContentQueryService, SensoryContentQueryService>();

// TODO: Uncomment as you build your Scheduling & Calendar Bounded Context (BC02)
// builder.Services.AddScoped<IShowRepository, ShowRepository>();
// builder.Services.AddScoped<IScheduleConflictRepository, ScheduleConflictRepository>();
// builder.Services.AddScoped<IShowCommandService, ShowCommandService>();
// builder.Services.AddScoped<IShowQueryService, ShowQueryService>();

// TODO: Uncomment as you build your Room & Resource Readiness Bounded Context (BC03)
// builder.Services.AddScoped<IRoomRepository, RoomRepository>();
// builder.Services.AddScoped<IRoomInspectionRepository, RoomInspectionRepository>();
// builder.Services.AddScoped<IRoomBlockRepository, RoomBlockRepository>();
// builder.Services.AddScoped<IRoomCommandService, RoomCommandService>();
// builder.Services.AddScoped<IRoomQueryService, RoomQueryService>();

// TODO: Uncomment as you build your Ticketing Integration Bounded Context (BC04)
// builder.Services.AddScoped<ITicketingIntegrationRepository, TicketingIntegrationRepository>();
// builder.Services.AddScoped<ISyncLogRepository, SyncLogRepository>();
// builder.Services.AddScoped<IShowOccupancyRepository, ShowOccupancyRepository>();
// builder.Services.AddScoped<ITicketingCommandService, TicketingCommandService>();
// builder.Services.AddScoped<ITicketingQueryService, TicketingQueryService>();

// TODO: Uncomment as you build your Seat Allocation & Control Bounded Context (BC05)
// builder.Services.AddScoped<ISeatAllocationRepository, SeatAllocationRepository>();
// builder.Services.AddScoped<IShowSeatRepository, ShowSeatRepository>();
// builder.Services.AddScoped<IManualInterventionRepository, ManualInterventionRepository>();
// builder.Services.AddScoped<ISeatAllocationCommandService, SeatAllocationCommandService>();
// builder.Services.AddScoped<ISeatAllocationQueryService, SeatAllocationQueryService>();

// TODO: Uncomment as you build your 4D Execution & Synchronization Bounded Context (BC06)
// builder.Services.AddScoped<IShowExecutionRepository, ShowExecutionRepository>();
// builder.Services.AddScoped<ISensorySequenceRepository, SensorySequenceRepository>();
// builder.Services.AddScoped<ISynchronizationEventRepository, SynchronizationEventRepository>();
// builder.Services.AddScoped<IEmergencyEventRepository, EmergencyEventRepository>();
// builder.Services.AddScoped<IShowExecutionCommandService, ShowExecutionCommandService>();
// builder.Services.AddScoped<IShowExecutionQueryService, ShowExecutionQueryService>();

// TODO: Uncomment as you build your Resource Testing & Maintenance Bounded Context (BC07)
// builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
// builder.Services.AddScoped<IIncidentRepository, IncidentRepository>();
// builder.Services.AddScoped<IMaintenanceOrderRepository, MaintenanceOrderRepository>();
// builder.Services.AddScoped<IEquipmentTestRepository, EquipmentTestRepository>();
// builder.Services.AddScoped<IMaintenanceCommandService, MaintenanceCommandService>();
// builder.Services.AddScoped<IMaintenanceQueryService, MaintenanceQueryService>();

// TODO: Uncomment as you build your Operational Analytics & Reporting Bounded Context (BC08)
// builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
// builder.Services.AddScoped<IOperationalMetricRepository, OperationalMetricRepository>();
// builder.Services.AddScoped<IRoomPerformanceRepository, RoomPerformanceRepository>();
// builder.Services.AddScoped<IAnalyticsQueryService, AnalyticsQueryService>();

// Mediator Configuration

// Add Mediator Injection Configuration
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));

// Add Cortex Mediator for Event Handling
builder.Services.AddCortexMediator(
    configuration: builder.Configuration,
    handlerAssemblyMarkerTypes: new[] { typeof(Program) },
    configure: options =>
    {
        options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>));
        //options.AddDefaultBehaviors();
    });

var app = builder.Build();

// Verify if the database exists and create it if it doesn't
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();