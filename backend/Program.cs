using CapabilityMap.Backend.Api.Controllers;
using CapabilityMap.Backend.Api.Controllers.Advisor;
using CapabilityMap.Backend.Api.Controllers.Customer;
using CapabilityMap.Backend.Api.Controllers.Evaluator;
using CapabilityMap.Backend.Application.Security.Areas;
using CapabilityMap.Backend.Application.Security.RoleAssignments;
using CapabilityMap.Backend.Infrastructure.Logging;
using CapabilityMap.Backend.Infrastructure.Persistence;
using CapabilityMap.Backend.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "http://localhost:8080", "http://localhost:8082")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<RoleAuthorizationService>();
builder.Services.AddSingleton<AuthorizationEventLogger>();
builder.Services.AddSingleton<IRoleAssignmentStore, InMemoryRoleAssignmentStore>();
builder.Services.AddSingleton<RoleAssignmentService>();
builder.Services.AddSingleton<IAreaStore, InMemoryAreaStore>();
builder.Services.AddSingleton<AreaManagementService>();
builder.Services.AddSingleton<AreaCreationService>();
builder.Services.AddSingleton<AreaPermissionService>();
builder.Services.AddSingleton<AreaEventLogger>();
builder.Services.AddSingleton<CustomerAccessPolicy>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "backend",
    utcTimestamp = DateTimeOffset.UtcNow,
}));

app.MapGet("/", () => Results.Ok(new
{
    name = "Capability Map Backend",
    endpoints = new[]
    {
        "/health",
        RoleAssignmentsController.Route,
        AreaController.Route,
    },
}));

app.MapRoleAssignmentEndpoints();
    app.MapAreaEndpoints();
app.MapAreaPermissionEndpoints();
app.MapCustomerContentEndpoints();
app.MapAdvisorEndpoints();
app.MapEvaluatorEndpoints();

app.Run();

public partial class Program;
