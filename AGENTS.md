# AGENTS.md

## Purpose
This file helps AI coding agents quickly understand the workspace and follow project conventions while working in the Payment Gateway POC.

## Project overview
- ASP.NET Core Web API proof-of-concept
- Uses Entity Framework Core with SQLite
- Follows a layered architecture:
  - `Controllers/` for HTTP endpoints
  - `Services/` for business logic
  - `Repositories/` for data access abstractions
  - `Models/` for entity definitions and validation attributes
  - `Data/` for EF Core `DbContext`

## Key conventions
- Keep controllers thin; delegate business behavior to services.
- Use dependency injection via `Program.cs` and register services with `AddScoped`.
- Prefer `Task`/`async` endpoints and service methods.
- Use `DataAnnotations` for model validation and DTOs where appropriate.
- Follow the coding conventions in `Documents/Coding Guideline.md`:
  - PascalCase for classes and methods
  - camelCase for local variables and fields
  - `Async` suffix for async methods
  - 4-space indentation, no tabs

## Important files
- `src/PaymentGatewayPOC/Program.cs` — application startup and DI registration
- `src/PaymentGatewayPOC/Controllers/` — REST API controllers
- `src/PaymentGatewayPOC/Services/` — service layer interfaces and implementations
- `src/PaymentGatewayPOC/Repositories/` — generic repository and unit of work
- `src/PaymentGatewayPOC/Models/` — domain entities and validation
- `src/PaymentGatewayPOC/Data/PaymentGatewayContext.cs` — EF Core database context

## Build and run
- Build: `dotnet build src/PaymentGatewayPOC/PaymentGatewayPOC.csproj`
- Run: `dotnet run --project src/PaymentGatewayPOC/PaymentGatewayPOC.csproj`

## Database
- Add a migration: `dotnet ef migrations add <MigrationName> --project src/PaymentGatewayPOC/PaymentGatewayPOC.csproj`
- Apply migrations: `dotnet ef database update --project src/PaymentGatewayPOC/PaymentGatewayPOC.csproj`

## What not to do
- Do not invent test projects or files that are not present in the workspace.
- Do not change the API versioning style or route conventions without a specific request.
- Do not assume a production deployment architecture; this is a POC.

## Additional documentation
- `README.md`
- `Documents/Coding Guideline.md`
- `Documents/Product Requirement Document.md`
- `Documents/Technical Requirement Document.md`
