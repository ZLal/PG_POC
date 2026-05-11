using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class ClientService : IClientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClientService> _logger;

    public ClientService(IUnitOfWork unitOfWork, ILogger<ClientService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all clients");
            var clients = await _unitOfWork.Clients.GetAllAsync();
            _logger.LogInformation($"Retrieved {clients.Count()} clients");
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
            _logger.LogInformation($"Fetching client with ID: {id}");
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"Client with ID {id} not found");
            }
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching client with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetClientsByOrganizationAsync(Guid organizationId)
    {
        try
        {
            _logger.LogInformation($"Fetching clients for organization ID: {organizationId}");
            var clients = await _unitOfWork.Clients.FindAsync(c => c.OrganizationId == organizationId);
            _logger.LogInformation($"Retrieved {clients.Count()} clients for organization {organizationId}");
            return clients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching clients for organization {organizationId}");
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetClientsByApplicationAsync(Guid applicationId)
    {
        try
        {
            _logger.LogInformation($"Fetching clients for application ID: {applicationId}");
            var clients = await _unitOfWork.Clients.FindAsync(c => c.ApplicationId == applicationId);
            _logger.LogInformation($"Retrieved {clients.Count()} clients for application {applicationId}");
            return clients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching clients for application {applicationId}");
            throw;
        }
    }

    public async Task<Client> CreateClientAsync(Guid organizationId, Guid applicationId, string name, string secretKey, DateTime? expiryDate = null)
    {
        try
        {
            _logger.LogInformation($"Creating new client for organization ID: {organizationId}, application ID: {applicationId}");
            
            var client = new Client
            {
                ClientId = Guid.NewGuid(),
                OrganizationId = organizationId,
                ApplicationId = applicationId,
                Name = name,
                SecretKey = secretKey,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = expiryDate
            };

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Client created successfully with ID: {client.ClientId}");
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating client for organization {organizationId}");
            throw;
        }
    }

    public async Task<Client> UpdateClientAsync(Guid id, string name, DateTime? expiryDate)
    {
        try
        {
            _logger.LogInformation($"Updating client with ID: {id}");
            
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null)
            {
                _logger.LogWarning($"Client with ID {id} not found for update");
                throw new KeyNotFoundException($"Client with ID {id} not found");
            }

            client.Name = name;
            client.ExpiryDate = expiryDate;
            await _unitOfWork.Clients.UpdateAsync(client);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Client with ID {id} updated successfully");
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating client with ID {id}");
            throw;
        }
    }

    public async Task<bool> DeleteClientAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Deleting client with ID: {id}");
            
            var result = await _unitOfWork.Clients.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Client with ID {id} deleted successfully");
            }
            else
            {
                _logger.LogWarning($"Client with ID {id} not found for deletion");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting client with ID {id}");
            throw;
        }
    }

    public async Task<bool> ValidateClientSecretAsync(Guid clientId, string secretKey)
    {
        try
        {
            _logger.LogInformation($"Validating secret key for client ID: {clientId}");
            
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
            if (client == null)
            {
                _logger.LogWarning($"Client with ID {clientId} not found for validation");
                return false;
            }

            var isValid = client.SecretKey == secretKey && (client.ExpiryDate == null || client.ExpiryDate > DateTime.UtcNow);
            
            if (!isValid)
            {
                _logger.LogWarning($"Invalid secret key or expired client for ID {clientId}");
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error validating secret key for client {clientId}");
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
