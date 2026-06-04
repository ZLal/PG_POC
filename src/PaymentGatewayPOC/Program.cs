using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Utilities;
using PaymentGatewayPOC.Utilities.Interfaces;
using PaymentGatewayPOC.Repositories;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services;
using PaymentGatewayPOC.Services.Interfaces;
using PaymentGatewayPOC.Components;
using PaymentGatewayPOC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add configuration for strongly typed settings
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PaymentGatewayPOC.Data.PaymentGatewayContext>(options =>
    // options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"))
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"))
);
builder.Services.AddScoped<IMigrationService, MigrationService>();

// Register repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services with logging
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IGatewayService, GatewayService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Register utilities
builder.Services.AddScoped<IRandomService, RandomService>();

// Register Blazor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Apply any pending EF Core migrations on startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var migrationService = services.GetRequiredService<IMigrationService>();
    await migrationService.MigrateDataAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.MapControllers();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
