using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

public interface IApplicationRepository : IRepository<Application>
{
    Task<Application?> GetApplicationByNameAsync(Guid organizationId, string name);
    Task<Application?> GetApplicationWithGatewayAsync(Guid applicationId);
}
