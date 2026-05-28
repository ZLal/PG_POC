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
    /// Get application by name
    /// </summary>
    Task<Application?> GetApplicationByNameAsync(Guid organizationId, string name);

    /// <summary>
    /// Get application with their associated gateways
    /// </summary>
    Task<Application?> GetApplicationWithGatewayAsync(Guid applicationId);

    /// <summary>
    /// Get application with their associated active gateways
    /// </summary>
    Task<Application?> GetApplicationWithActiveGatewayAsync(Guid applicationId);

    /// <summary>
    /// Create a new application
    /// </summary>
    Task<Application> CreateApplicationAsync(Guid organizationId, string name);

    /// <summary>
    /// Update application
    /// </summary>
    Task<Application> UpdateApplicationAsync(Guid id, string name);

    /// <summary>
    /// Delete application
    /// </summary>
    Task<bool> DeleteApplicationAsync(Guid id);

    /// <summary>
    /// Get applications count
    /// </summary>
    Task<int> GetApplicationCountAsync();
}
