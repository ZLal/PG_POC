using Microsoft.EntityFrameworkCore;
using PaymentGatewayPOC.Data;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories;

public class ApplicationRepository : Repository<Application>, IApplicationRepository
{
    public ApplicationRepository(PaymentGatewayContext context) : base(context)
    {
    }

    public async Task<Application?> GetApplicationWithGatewayAsync(Guid applicationId)
    {
        return await _context.Applications
            .Include(a => a.ApplicationGateways)
                .ThenInclude(ag => ag.Gateway)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
    }
}
