using IncidentManagement.API;
using IncidentManagement.Application;
using IncidentManagement.Infrastructure;
using IncidentManagement.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, one layer at a time.
builder.Services.AddWebApiServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(IncidentManagement.API.DependencyInjection.ReactClientCorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<IncidentHub>("/hubs/incidents");

app.Run();
