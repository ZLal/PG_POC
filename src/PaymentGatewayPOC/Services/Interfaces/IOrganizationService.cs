using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Services.Interfaces;

public interface IOrganizationService
{
    /// <summary>
    /// Get all organizations
    /// </summary>
    Task<IEnumerable<Organization>> GetAllOrganizationsAsync();

    /// <summary>
    /// Get organization by ID
    /// </summary>
    Task<Organization?> GetOrganizationByIdAsync(Guid id);

    /// <summary>
    /// Create a new organization
    /// </summary>
    Task<Organization> CreateOrganizationAsync(string name);

    /// <summary>
    /// Update organization
    /// </summary>
    Task<Organization> UpdateOrganizationAsync(Guid id, string name);

    /// <summary>
    /// Delete organization
    /// </summary>
    Task<bool> DeleteOrganizationAsync(Guid id);

    /// <summary>
    /// Get organizations count
    /// </summary>
    Task<int> GetOrganizationCountAsync();
}
