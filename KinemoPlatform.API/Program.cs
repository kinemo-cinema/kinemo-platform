// TODO: Uncomment these usings as you create the corresponding files in your Shared folder
// using KinemoPlatform.API.Shared.Domain.Repositories;
// using KinemoPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
// using KinemoPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
// using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
// using KinemoPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
// using Cortex.Mediator.Commands;
// using Cortex.Mediator.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// TODO: Add the KebabCaseRouteNamingConvention options once you create that file in Shared.
// builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// TODO: Uncomment the Database Context setup once you create AppDbContext in Shared/Infrastructure/Persistence/EFC/Configuration
/*
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
*/

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO: Uncomment this block when you are ready to configure your Swagger metadata
/*
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "KinemoPlatform.API",
            Version = "v1",
            Description = "Kinemo Platform API",
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
*/

// Dependency Injection

// TODO: Uncomment as you build your Shared Kernel
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// TODO: Uncomment as you build your Publishing Bounded Context
// builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ITutorialRepository, TutorialRepository>();
// builder.Services.AddScoped<ICategoryCommandService, CategoryCommandService>();
// builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
// builder.Services.AddScoped<ITutorialCommandService, TutorialCommandService>();
// builder.Services.AddScoped<ITutorialQueryService, TutorialQueryService>();

// TODO: Uncomment as you build your Profiles Bounded Context
// builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
// builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
// builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();

// TODO: Uncomment as you build your Mediator Configuration
// builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
// builder.Services.AddCortexMediator(
//     configuration: builder.Configuration,
//     handlerAssemblyMarkerTypes: [typeof(Program)], configure: options =>
//     {
//         options.AddOpenCommandPipelineBehavior(typeof(LoggingCommandBehavior<>));
//     });

var app = builder.Build();

// TODO: Uncomment this block ONLY AFTER you create AppDbContext and set up your connection string.
// This is what ensures your MySQL database is created if it doesn't exist.
/*
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}
*/

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