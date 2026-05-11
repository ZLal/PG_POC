using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrganizationService> _logger;

    public OrganizationService(IUnitOfWork unitOfWork, ILogger<OrganizationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Organization>> GetAllOrganizationsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all organizations");
            var organizations = await _unitOfWork.Organizations.GetAllAsync();
            _logger.LogInformation($"Retrieved {organizations.Count()} organizations");
            return organizations;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all organizations");
            throw;
        }
    }

    public async Task<Organization?> GetOrganizationByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching organization with ID: {id}");
            var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
            if (organization == null)
            {
                _logger.LogWarning($"Organization with ID {id} not found");
            }
            return organization;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching organization with ID {id}");
            throw;
        }
    }

    public async Task<Organization> CreateOrganizationAsync(string name)
    {
        try
        {
            _logger.LogInformation($"Creating new organization with name: {name}");
            
            var organization = new Organization
            {
                OrganizationId = Guid.NewGuid(),
                Name = name,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Organizations.AddAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Organization created successfully with ID: {organization.OrganizationId}");
            return organization;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating organization with name: {name}");
            throw;
        }
    }

    public async Task<Organization> UpdateOrganizationAsync(Guid id, string name)
    {
        try
        {
            _logger.LogInformation($"Updating organization with ID: {id}");
            
            var organization = await _unitOfWork.Organizations.GetByIdAsync(id);
            if (organization == null)
            {
                _logger.LogWarning($"Organization with ID {id} not found for update");
                throw new KeyNotFoundException($"Organization with ID {id} not found");
            }

            organization.Name = name;
            await _unitOfWork.Organizations.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Organization with ID {id} updated successfully");
            return organization;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating organization with ID {id}");
            throw;
        }
    }

    public async Task<bool> DeleteOrganizationAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Deleting organization with ID: {id}");
            
            var result = await _unitOfWork.Organizations.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Organization with ID {id} deleted successfully");
            }
            else
            {
                _logger.LogWarning($"Organization with ID {id} not found for deletion");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting organization with ID {id}");
            throw;
        }
    }

    public async Task<int> GetOrganizationCountAsync()
    {
        try
        {
            _logger.LogInformation("Fetching organization count");
            var count = await _unitOfWork.Organizations.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching organization count");
            throw;
        }
    }
}
