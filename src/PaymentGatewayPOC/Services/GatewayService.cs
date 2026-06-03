using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class GatewayService : IGatewayService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GatewayService> _logger;

    public GatewayService(IUnitOfWork unitOfWork, ILogger<GatewayService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Gateway>> GetAllGatewaysAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all gateways");
            var gateways = await _unitOfWork.Gateways.GetAllAsync();
            var gatewaysCount = gateways.Count();
            _logger.LogInformation("Retrieved {GatewaysCount} gateways", gatewaysCount);
            return gateways;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all gateways");
            throw;
        }
    }

    public async Task<Gateway?> GetGatewayByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching gateway with ID: {GatewayId}", id);
            var gateway = await _unitOfWork.Gateways.GetByIdAsync(id);
            if (gateway == null)
            {
                _logger.LogWarning("Gateway with ID {GatewayId} not found", id);
            }
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching gateway with ID {GatewayId}", id);
            throw;
        }
    }

    public async Task<Gateway?> GetGatewayByIdWithGatewayDetailsAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Fetching gateway with ID: {GatewayId} including details", id);
            var gateway = await _unitOfWork.Gateways.GetByIdWithChildrenAsync(g => g.GatewayId, id, g => g.ApplicationGateways);
            if (gateway == null)
            {
                _logger.LogWarning("Gateway with ID {GatewayId} not found", id);
            }
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching gateway with ID {GatewayId} including details", id);
            throw;
        }
    }

    public async Task<IEnumerable<Gateway>> GetActiveGatewaysAsync()
    {
        try
        {
            _logger.LogInformation("Fetching active gateways");
            var gateways = await _unitOfWork.Gateways.FindAsync(g => g.Status == GatewayStatus.Active);
            var activeGatewaysCount = gateways.Count();
            _logger.LogInformation("Retrieved {ActiveGatewaysCount} active gateways", activeGatewaysCount);
            return gateways;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active gateways");
            throw;
        }
    }

    public async Task<Gateway> CreateGatewayAsync(string name, GatewayStatus status = GatewayStatus.Active)
    {
        try
        {
            _logger.LogInformation("Creating new gateway with name: {Name}, status: {Status}", name, status);
            
            var gateway = new Gateway
            {
                GatewayId = Guid.NewGuid(),
                Name = name,
                Status = status
            };

            await _unitOfWork.Gateways.AddAsync(gateway);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Gateway created successfully with ID: {GatewayId}", gateway.GatewayId);
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating gateway with name: {Name}", name);
            throw;
        }
    }

    public async Task<Gateway> UpdateGatewayAsync(Guid id, string name, GatewayStatus status)
    {
        try
        {
            _logger.LogInformation("Updating gateway with ID: {GatewayId}", id);
            
            var gateway = await _unitOfWork.Gateways.GetByIdAsync(id);
            if (gateway == null)
            {
                _logger.LogWarning("Gateway with ID {GatewayId} not found for update", id);
                throw new KeyNotFoundException($"Gateway with ID {id} not found");
            }

            gateway.Name = name;
            gateway.Status = status;
            await _unitOfWork.Gateways.UpdateAsync(gateway);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Gateway with ID {GatewayId} updated successfully", id);
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating gateway with ID {GatewayId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteGatewayAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Deleting gateway with ID: {GatewayId}", id);
            
            var result = await _unitOfWork.Gateways.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Gateway with ID {GatewayId} deleted successfully", id);
            }
            else
            {
                _logger.LogWarning("Gateway with ID {GatewayId} not found for deletion", id);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting gateway with ID {GatewayId}", id);
            throw;
        }
    }

    public async Task<int> GetGatewayCountAsync()
    {
        try
        {
            _logger.LogInformation("Fetching gateway count");
            var count = await _unitOfWork.Gateways.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching gateway count");
            throw;
        }
    }
}
