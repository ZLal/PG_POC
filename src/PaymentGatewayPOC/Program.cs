using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Repositories;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services;
using PaymentGatewayPOC.Services.Interfaces;
using PaymentGatewayPOC.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PaymentGatewayPOC.Data.PaymentGatewayContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository pattern
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register services with logging
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IGatewayService, GatewayService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Register Blazor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Apply any pending EF Core migrations on startup.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<PaymentGatewayPOC.Data.PaymentGatewayContext>();
        // context.Database.EnsureCreated(); // Ensure the database is created before applying migrations
        context.Database.Migrate();
        logger.LogInformation("Database migrated successfully on startup.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database on startup.");
        throw;
    }
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
