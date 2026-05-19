using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Services.Interfaces;

public interface IClientService
{
    /// <summary>
    /// Get all clients
    /// </summary>
    Task<IEnumerable<Client>> GetAllClientsAsync();

    /// <summary>
    /// Get client by ID
    /// </summary>
    Task<Client?> GetClientByIdAsync(Guid id);

    /// <summary>
    /// Get clients by organization ID
    /// </summary>
    Task<IEnumerable<Client>> GetClientsByOrganizationAsync(Guid organizationId);

    /// <summary>
    /// Get clients by application ID
    /// </summary>
    Task<IEnumerable<Client>> GetClientsByApplicationAsync(Guid applicationId);

    /// <summary>
    /// Create a new client
    /// </summary>
    Task<Client> CreateClientAsync(Guid organizationId, Guid applicationId, string name, string secretKey, DateTime? expiryDate = null);

    /// <summary>
    /// Update client
    /// </summary>
    Task<Client> UpdateClientAsync(Guid id, string name, DateTime? expiryDate);

    /// <summary>
    /// Delete client
    /// </summary>
    Task<bool> DeleteClientAsync(Guid id);

    /// <summary>
    /// Check if client secret is valid
    /// </summary>
    Task<bool> ValidateClientSecretAsync(Guid clientId, string secretKey);

    /// <summary>
    /// Get client by secret key
    /// </summary>
    Task<Client?> GetClientBySecretKeyAsync(Guid clientId, string secretKey);

    /// <summary>
    /// Get clients count
    /// </summary>
    Task<int> GetClientCountAsync();
}
