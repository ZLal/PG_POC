using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Data;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories;

public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(PaymentGatewayContext context) : base(context)
    {
    }

    public async Task<Organization?> GetOrganizationByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(o => o.Name == name);
    }
}
