using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Utilities.Interfaces;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class ClientService(IUnitOfWork unitOfWork, ILogger<ClientService> logger, IRandomService randomService) : IClientService
{
    private const int SecretKeyLength = 32;
    private const int ExpiryDays = 365; // Default expiry of 1 year
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ClientService> _logger = logger;

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all clients");
            var clients = await _unitOfWork.Clients.GetAllAsync();
            _logger.LogInformation("Retrieved {ClientCount} clients", clients.Count());
            return clients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all clients");
            throw;
        }
    }

    public async Task<Client?> GetClientByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching client with ID: {ClientId}", id);
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null)
            {
                _logger.LogWarning("Client with ID {ClientId} not found", id);
            }
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching client with ID {ClientId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetClientsByOrganizationAsync(Guid organizationId)
    {
        try
        {
            _logger.LogInformation("Fetching clients for organization ID: {OrganizationId}", organizationId);
            var clients = await _unitOfWork.Clients.FindAsync(c => c.OrganizationId == organizationId);
            _logger.LogInformation("Retrieved {ClientCount} clients for organization {OrganizationId}", clients.Count(), organizationId);
            return clients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching clients for organization {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetClientsByApplicationAsync(Guid applicationId)
    {
        try
        {
            _logger.LogInformation("Fetching clients for application ID: {ApplicationId}", applicationId);
            var clients = await _unitOfWork.Clients.FindAsync(c => c.ApplicationId == applicationId);
            _logger.LogInformation("Retrieved {ClientCount} clients for application {ApplicationId}", clients.Count(), applicationId);
            return clients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching clients for application {ApplicationId}", applicationId);
            throw;
        }
    }

    public async Task<Client?> GetClientByNameAsync(Guid applicationId, string name)
    {
        try
        {
            _logger.LogInformation("Fetching client by name: {ClientName} for application ID: {ApplicationId}", name, applicationId);
            var client = await _unitOfWork.Clients.FindAsync(c => c.ApplicationId == applicationId && c.Name == name)
                .ContinueWith(t => t.Result.FirstOrDefault());
            if (client == null)
            {
                _logger.LogWarning("Client with name '{ClientName}' not found for application ID {ApplicationId}", name, applicationId);
            }
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching client by name '{ClientName}' for application ID {ApplicationId}", name, applicationId);
            throw;
        }
    }

    public async Task<Client> CreateClientAsync(Guid organizationId, Guid applicationId, string name)
    {
        try
        {
            _logger.LogInformation("Creating new client for organization ID: {OrganizationId}, application ID: {ApplicationId}", organizationId, applicationId);
            
            string secretKey = randomService.GenerateRandomString(SecretKeyLength);
            DateTime createdDate = DateTime.UtcNow;
            var client = new Client
            {
                ClientId = Guid.NewGuid(),
                OrganizationId = organizationId,
                ApplicationId = applicationId,
                Name = name,
                SecretKey = secretKey,
                CreatedDate = createdDate,
                ExpiryDate = createdDate.AddDays(ExpiryDays)
            };

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Client created successfully with ID: {ClientId}", client.ClientId);
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client for organization {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<Client> UpdateClientAsync(Guid id, string name, DateTime? expiryDate)
    {
        try
        {
            _logger.LogInformation("Updating client with ID: {ClientId}", id);
            
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null)
            {
                _logger.LogWarning("Client with ID {ClientId} not found for update", id);
                throw new KeyNotFoundException($"Client with ID {id} not found");
            }

            client.Name = name;
            client.ExpiryDate = expiryDate;
            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Client with ID {ClientId} updated successfully", id);
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating client with ID {ClientId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteClientAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting client with ID: {ClientId}", id);
            
            var result = await _unitOfWork.Clients.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Client with ID {ClientId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Client with ID {ClientId} not found for deletion", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting client with ID {ClientId}", id);
            throw;
        }
    }

    public async Task<bool> ValidateClientSecretAsync(Guid clientId, string secretKey)
    {
        try
        {
            _logger.LogInformation("Validating secret key for client ID: {ClientId}", clientId);
            
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID {ClientId} not found for validation", clientId);
                return false;
            }

            var isValid = client.SecretKey == secretKey && (client.ExpiryDate == null || client.ExpiryDate > DateTime.UtcNow);
            
            if (!isValid)
            {
                _logger.LogWarning("Invalid secret key or expired client for ID {ClientId}", clientId);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating secret key for client {ClientId}", clientId);
            throw;
        }
    }

    public async Task<Client?> GetClientBySecretKeyAsync(Guid clientId, string secretKey)
    {
        try
        {
            _logger.LogInformation("Fetching client by application for client ID: {ClientId}", clientId);
            
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning("Client with ID {ClientId} not found", clientId);
                return null;
            }

            if (client.SecretKey != secretKey || (client.ExpiryDate != null && client.ExpiryDate <= DateTime.UtcNow))
            {
                _logger.LogWarning("Invalid secret key or expired client for ID {ClientId}", clientId);
                return null;
            }

            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching client by application for ID {ClientId}", clientId);
            throw;
        }
    }

    public async Task<int> GetClientCountAsync()
    {
        try
        {
            _logger.LogInformation("Fetching client count");
            var count = await _unitOfWork.Clients.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching client count");
            throw;
        }
    }
}
