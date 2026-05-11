using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Services.Interfaces;

public interface IApplicationService
{
    /// <summary>
    /// Get all applications
    /// </summary>
    Task<IEnumerable<Application>> GetAllApplicationsAsync();

    /// <summary>
    /// Get application by ID
    /// </summary>
    Task<Application?> GetApplicationByIdAsync(Guid id);

    /// <summary>
    /// Get applications by organization ID
    /// </summary>
    Task<IEnumerable<Application>> GetApplicationsByOrganizationAsync(Guid organizationId);

    /// <summary>
    /// Create a new application
    /// </summary>
    Task<Application> CreateApplicationAsync(Guid organizationId, string clientId, string accessLocation);

    /// <summary>
    /// Update application
    /// </summary>
    Task<Application> UpdateApplicationAsync(Guid id, string clientId, string accessLocation);

    /// <summary>
    /// Delete application
    /// </summary>
    Task<bool> DeleteApplicationAsync(Guid id);

    /// <summary>
    /// Get applications count
    /// </summary>
    Task<int> GetApplicationCountAsync();
}
