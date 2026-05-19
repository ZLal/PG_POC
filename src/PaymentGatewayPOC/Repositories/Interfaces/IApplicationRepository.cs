
public interface IApplicationRepository : IRepository<Application>
{
    Task<IEnumerable<Application>> GetApplicationsWithGatewayAsync(Guid applicationId);
    Task<IEnumerable<Application>> GetApplicationsWithActiveGatewayAsync(Guid applicationId);
}
