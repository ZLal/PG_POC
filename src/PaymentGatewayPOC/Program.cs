using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Repositories;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services;
using PaymentGatewayPOC.Services.Interfaces;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
