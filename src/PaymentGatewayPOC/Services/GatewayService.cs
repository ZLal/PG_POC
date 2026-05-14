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
            _logger.LogInformation($"Retrieved {gateways.Count()} gateways");
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
            _logger.LogInformation($"Fetching gateway with ID: {id}");
            var gateway = await _unitOfWork.Gateways.GetByIdAsync(id);
            if (gateway == null)
            {
                _logger.LogWarning($"Gateway with ID {id} not found");
            }
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching gateway with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<Gateway>> GetActiveGatewaysAsync()
    {
        try
        {
            _logger.LogInformation("Fetching active gateways");
            var gateways = await _unitOfWork.Gateways.FindAsync(g => g.Status == GatewayStatus.Active);
            _logger.LogInformation($"Retrieved {gateways.Count()} active gateways");
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
            _logger.LogInformation($"Creating new gateway with name: {name}, status: {status}");
            
            var gateway = new Gateway
            {
                GatewayId = Guid.NewGuid(),
                Name = name,
                Status = status
            };

            await _unitOfWork.Gateways.AddAsync(gateway);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Gateway created successfully with ID: {gateway.GatewayId}");
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating gateway with name: {name}");
            throw;
        }
    }

    public async Task<Gateway> UpdateGatewayAsync(Guid id, string name, GatewayStatus status)
    {
        try
        {
            _logger.LogInformation($"Updating gateway with ID: {id}");
            
            var gateway = await _unitOfWork.Gateways.GetByIdAsync(id);
            if (gateway == null)
            {
                _logger.LogWarning($"Gateway with ID {id} not found for update");
                throw new KeyNotFoundException($"Gateway with ID {id} not found");
            }

            gateway.Name = name;
            gateway.Status = status;
            await _unitOfWork.Gateways.UpdateAsync(gateway);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Gateway with ID {id} updated successfully");
            return gateway;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating gateway with ID {id}");
            throw;
        }
    }

    public async Task<bool> DeleteGatewayAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Deleting gateway with ID: {id}");
            
            var result = await _unitOfWork.Gateways.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Gateway with ID {id} deleted successfully");
            }
            else
            {
                _logger.LogWarning($"Gateway with ID {id} not found for deletion");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting gateway with ID {id}");
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
