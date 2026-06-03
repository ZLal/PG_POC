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
            _logger.LogInformation("Retrieved {count} applications", applications.Count());
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
            _logger.LogInformation("Fetching application with ID: {ApplicationId}", id);
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found", id);
            }
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching application with ID {ApplicationId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Application>> GetApplicationsByOrganizationAsync(Guid organizationId)
    {
        try
        {
            _logger.LogInformation("Fetching applications for organization ID: {OrganizationId}", organizationId);
            var applications = await _unitOfWork.Applications.FindAsync(a => a.OrganizationId == organizationId);
            _logger.LogInformation("Retrieved {count} applications for organization {OrganizationId}", applications.Count(), organizationId);
            return applications;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching applications for organization {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<Application?> GetApplicationByNameAsync(Guid organizationId, string name)
    {
        try
        {
            _logger.LogInformation("Fetching application with name: {ApplicationName}", name);
            var application = await _unitOfWork.Applications.GetApplicationByNameAsync(organizationId, name);
            if (application == null)
            {
                _logger.LogWarning("Application with name {ApplicationName} not found", name);
            }
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching application with name {ApplicationName}", name);
            throw;
        }
    }

    public async Task<Application?> GetApplicationWithGatewayAsync(Guid applicationId)
    {
        try
        {
            _logger.LogInformation("Fetching application with gateways for application ID: {ApplicationId}", applicationId);
            var application = await _unitOfWork.Applications.GetApplicationWithGatewayAsync(applicationId);
            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found", applicationId);
            }
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching application with gateways for application ID {ApplicationId}", applicationId);
            throw;
        }
    }

    public async Task<Application?> GetApplicationWithActiveGatewayAsync(Guid applicationId)
    {
        try
        {
            _logger.LogInformation("Fetching application with active gateways for application ID: {ApplicationId}", applicationId);
            var application = await _unitOfWork.Applications.GetApplicationWithGatewayAsync(applicationId);
            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found", applicationId);
            }
            else
            {
                application.ApplicationGateways = [.. application.ApplicationGateways
                    .Where(ag => ag.Gateway != null && ag.Gateway.Status == GatewayStatus.Active)];
            }
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching application with active gateways for application ID {ApplicationId}", applicationId);
            throw;
        }
    }

    public async Task<Application> CreateApplicationAsync(Guid organizationId, string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Application name is required");
                throw new ArgumentException("Application name is required", nameof(name));
            }
            _logger.LogInformation("Creating new application for organization ID: {OrganizationId}", organizationId);
            
            var application = new Application
            {
                ApplicationId = Guid.NewGuid(),
                OrganizationId = organizationId,
                Name = name,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Application created successfully with ID: {ApplicationId}", application.ApplicationId);
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application for organization {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<Application> UpdateApplicationAsync(Guid id, string name)
    {
        try
        {
            _logger.LogInformation("Updating application with ID: {ApplicationId}", id);
            
            var application = await _unitOfWork.Applications.GetByIdAsync(id);
            if (application == null)
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found for update", id);
                throw new KeyNotFoundException($"Application with ID {id} not found");
            }

            application.Name = name;
            await _unitOfWork.Applications.UpdateAsync(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Application with ID {ApplicationId} updated successfully", id);
            return application;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application with ID {ApplicationId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteApplicationAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting application with ID: {ApplicationId}", id);
            
            var result = await _unitOfWork.Applications.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Application with ID {ApplicationId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Application with ID {ApplicationId} not found for deletion", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting application with ID {ApplicationId}", id);
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
