using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class ApplicationService : IApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApplicationService> _logger;

    public ApplicationService(IUnitOfWork unitOfWork, ILogger<ApplicationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all applications");
            var applications = await _unitOfWork.Applications.GetAllAsync();
            _logger.LogInformation($"Retrieved {applications.Count()} applications");
            return applications;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all applications");
            throw;
        }
    }

    public async Task<Application?> GetApplicationByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching application with ID: {id}");
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
            {
                _logger.LogWarning($"Application with ID {id} not found");
            }
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching application with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<Application>> GetApplicationsByOrganizationAsync(Guid organizationId)
    {
        try
        {
            _logger.LogInformation($"Fetching applications for organization ID: {organizationId}");
            var applications = await _unitOfWork.Applications.FindAsync(a => a.OrganizationId == organizationId);
            _logger.LogInformation($"Retrieved {applications.Count()} applications for organization {organizationId}");
            return applications;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching applications for organization {organizationId}");
            throw;
        }
    }

    public async Task<Application> CreateApplicationAsync(Guid organizationId, string clientId, string accessLocation)
    {
        try
        {
            _logger.LogInformation($"Creating new application for organization ID: {organizationId}");
            
            var application = new Application
            {
                ApplicationId = Guid.NewGuid(),
                OrganizationId = organizationId,
                ClientId = clientId,
                AccessLocation = accessLocation,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Application created successfully with ID: {application.ApplicationId}");
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating application for organization {organizationId}");
            throw;
        }
    }

    public async Task<Application> UpdateApplicationAsync(Guid id, string clientId, string accessLocation)
    {
        try
        {
            _logger.LogInformation($"Updating application with ID: {id}");
            
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
            {
                _logger.LogWarning($"Application with ID {id} not found for update");
                throw new KeyNotFoundException($"Application with ID {id} not found");
            }

            application.ClientId = clientId;
            application.AccessLocation = accessLocation;
            await _unitOfWork.Applications.UpdateAsync(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Application with ID {id} updated successfully");
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating application with ID {id}");
            throw;
        }
    }

    public async Task<bool> DeleteApplicationAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Deleting application with ID: {id}");
            
            var result = await _unitOfWork.Applications.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Application with ID {id} deleted successfully");
            }
            else
            {
                _logger.LogWarning($"Application with ID {id} not found for deletion");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting application with ID {id}");
            throw;
        }
    }

    public async Task<int> GetApplicationCountAsync()
    {
        try
        {
            _logger.LogInformation("Fetching application count");
            var count = await _unitOfWork.Applications.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching application count");
            throw;
        }
    }
}
