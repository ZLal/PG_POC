using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

public interface IOrganizationRepository : IRepository<Organization>
{
    Task<Organization?> GetOrganizationByNameAsync(string name);
}
