using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

public interface IClientRepository : IRepository<Client>
{
    Task<Client?> GetClientByNameAsync(Guid applicationId, string name);
}
