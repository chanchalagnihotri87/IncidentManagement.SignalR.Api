# Project: MyTodo Personal Assistant

## What this project does
Asp.Net Api project with Clean Architecture for a Incident Management System. The project is designed to help users manage their tasks and incidents efficiently. It provides a user-friendly interface chatting with users, track incidents statuses, send notifications incidently. 


## Tech Stack
- ASP.NET Core MVC, .Net 10
- Entity Framework Core + SQL Server (Server Name: localhost\\MSSQLSERVER01, Database Name: MyTodoDB_GenerateClaudeMdByOwn)


## Project Structure
- `IncidentManagement.API` : The main ASP.NET Api project that contains the controllers, middleware, and configuration files.
- `IncidentManagement.Application` : Contains the application layer, including services, interfaces, and DTOs.
- `IncidentManagement.Domain` : Contains the domain layer, including entities, value objects, and domain services.
- `IncidentManagement.Infrastructure` : Contains the infrastructure layer, including data access, repositories, and external services.


## Coding Conventions
- Follow PascalCase for naming public members and methods. Use _camelCase for private fields
- Use meaningful names for variables, methods, and classes to improve code readability.
- Use async/await for asynchronous operations to improve performance and responsiveness.

## Logging Standards

### 1. Use Microsoft.Extensions.Logging

Use the built-in `ILogger<T>` abstraction.

```csharp
private readonly ILogger<HomeController> _logger;

public HomeController(ILogger<HomeController> logger)
{
    _logger = logger;
}

## Commands
- `dotnet build` : Build the application
- `dotnet run --project MyTodo` : Run the application
- `dotnet ef migrations add <MigrationName>` : Add a new migration
- `dotnet ef database update` : Apply migrations to the database


## Don'ts
- Don't modify `Migrations` folder manually. Use EF Core commands to manage migrations.
- Don't add nuget packages without asking.
- Don't do SignalR work, I will do it myself, you can help me with the backend logic and API endpoints.


## PR Review Rules

When reviewing a PR:
- Understand the business intent first.
- Review the diff in the context of the existing codebase.
- Prioritize real defects over style preferences.
- Check security, performance, reliability, concurrency, database, API,
  DI, error handling, logging, and test coverage.
- Report severity, location, problem, impact, and recommended fix.
- Do not modify files unless explicitly asked.
- Explain the reasoning behind important findings.