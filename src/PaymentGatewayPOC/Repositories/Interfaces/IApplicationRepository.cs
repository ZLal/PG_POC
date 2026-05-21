using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

public interface IApplicationRepository : IRepository<Application>
{
    Task<Application?> GetApplicationWithGatewayAsync(Guid applicationId);
}
