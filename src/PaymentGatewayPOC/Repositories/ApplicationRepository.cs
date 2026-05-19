
public class ApplicationRepository : Repository<Application>, IApplicationRepository
{
    public ApplicationRepository(PaymentGatewayContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Application>> GetApplicationsWithGatewayAsync(Guid applicationId)
    {
        return await _context.Applications
            .Include(a => a.ApplicationGateways)
                .ThenInclude(ag => ag.Gateway)
            .Where(a => a.ApplicationId == applicationId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetApplicationsWithActiveGatewayAsync(Guid applicationId)
    {
        return await _context.Applications
            .Include(a => a.ApplicationGateways)
                .ThenInclude(ag => ag.Gateway)
            .Where(a => a.ApplicationId == applicationId && 
                        a.ApplicationGateways.Any(ag => ag.Gateway != null && ag.Gateway.IsActive))
            .ToListAsync();
    }
}
