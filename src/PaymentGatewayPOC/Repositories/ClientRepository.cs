using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Data;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories;

public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(PaymentGatewayContext context) : base(context)
    {
    }

    public async Task<Client?> GetClientByNameAsync(Guid applicationId, string name)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.ApplicationId == applicationId && c.Name == name);
    }
}
