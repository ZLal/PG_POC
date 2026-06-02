using PaymentGatewayPOC.Utilities.Interfaces;
using PaymentGatewayPOC.Data;
using Microsoft.EntityFrameworkCore;

namespace PaymentGatewayPOC.Utilities;

public class MigrationService(ILogger<MigrationService> logger, PaymentGatewayContext context) : IMigrationService
{
    public Task MigrateDataAsync()
    {
        try
        {
            context.Database.Migrate();
            logger.LogInformation("Database migrated successfully on startup.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database on startup.");
            throw;
        }
        return Task.CompletedTask;
    }
}
