using IncidentManagement.API;
using IncidentManagement.Application;
using IncidentManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, one layer at a time.
builder.Services.AddWebApiServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

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

app.Run();
