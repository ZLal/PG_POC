using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PaymentGatewayPOC;
using PaymentGatewayPOC.Utilities.Interfaces;
using Moq;

namespace PaymentGatewayPOC.UnitTests.TestHelper;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<IServiceCollection>? _overrideDependencies;

    public CustomWebApplicationFactory(Action<IServiceCollection>? overrideDependencies = null)
    {
        _overrideDependencies = overrideDependencies;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => _overrideDependencies?.Invoke(services));
    }

    public static async Task<CustomWebApplicationFactory> CreateDefaultApplicationAsync()
    {
        await using var appFactory = new CustomWebApplicationFactory(services =>
        {
            services.Mock<IMigrationService>(mock => mock.Setup(x => x.MigrateDataAsync()).Returns(Task.CompletedTask));
        });
        return appFactory;
    }
}