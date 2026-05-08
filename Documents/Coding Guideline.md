## .NET C# Coding Guidelines

### 1. General Coding Style
### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes & Methods | PascalCase | CustomerService, GetInvoice() |
| Variables & Fields | camelCase | invoiceTotal, customerName |
| Constants | ALLCAPSWITHUNDERSCORES | MAXRETRY_COUNT |
| Async Methods | Suffix with Async | SendEmailAsync() |

### Formatting
- Use 4 spaces for indentation (no tabs)
- Keep line length ≤ 120 characters
- Place braces on new lines (Microsoft convention):

```csharp
if (isValid)
{
    Process();
}
```

### Null Handling

Use ?? and ?. operators for concise null checks:

```csharp
var length = input?.Length ?? 0;
```

### 2. Language Features
- Prefer var for local variables when the type is obvious
- Use record types for immutable DTOs:

```csharp
public record CustomerDto(string Name, int Age);
```

- Use pattern matching for cleaner conditionals:

```csharp
if (obj is Customer c && c.IsActive)
{
    // Handle active customer
}
```

### 3. Architecture & Design Patterns
### Layered Architecture
- Separate Domain, Application, Infrastructure, and Presentation layers
- Keep business logic out of controllers

### Dependency Injection (DI)

Use the built-in DI container (Microsoft.Extensions.DependencyInjection) and register services with appropriate lifetimes:

| Lifetime | Use Case |
|----------|----------|
| AddSingleton | Stateless services |
| AddScoped | Per-request services |
| AddTransient | Lightweight, short-lived services |

### Microservices
- Use an API Gateway for routing and aggregation
- Prefer async communication (e.g., message queues) for scalability
- Apply Circuit Breaker and Retry patterns for resilience

### Design Patterns

| Pattern | Purpose |
|---------|---------|
| Repository | Data access abstraction |
| Factory | Complex object creation |

### 4. Error Handling & Logging
- Use structured logging with Serilog or Microsoft.Extensions.Logging
- Avoid swallowing exceptions — always log or rethrow
- Use global exception handling middleware in ASP.NET Core

### Security & Performance
- Validate input models with FluentValidation or DataAnnotations
- Use async/await for I/O-bound operations
- Apply caching (e.g., MemoryCache, Redis) for expensive queries
- Secure secrets with Azure Key Vault or dotnet user-secrets

### 6. Tooling & Practices
### Static Analysis
- Enable Roslyn Analyzers and StyleCop
- Use SonarQube or SonarCloud for enterprise projects

### Testing

| Type | Tool |
|------|------|
| Unit Tests | xUnit |
| Mocking | Moq |
| Integration Tests | WebApplicationFactory |

### CI/CD
- Use GitHub Actions or Azure DevOps pipelines
- Automate builds, tests, and deployments

### Documentation
- Use XML comments for public APIs
- Generate API docs with Swashbuckle (Swagger)
