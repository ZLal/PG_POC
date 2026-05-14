using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Services.Interfaces;

public interface IGatewayService
{
    /// <summary>
    /// Get all gateways
    /// </summary>
    Task<IEnumerable<Gateway>> GetAllGatewaysAsync();

    /// <summary>
    /// Get gateway by ID
    /// </summary>
    Task<Gateway?> GetGatewayByIdAsync(Guid id);

    /// <summary>
    /// Get active gateways only
    /// </summary>
    Task<IEnumerable<Gateway>> GetActiveGatewaysAsync();

    /// <summary>
    /// Create a new gateway
    /// </summary>
    Task<Gateway> CreateGatewayAsync(string name, GatewayStatus status = GatewayStatus.Active);

    /// <summary>
    /// Update gateway
    /// </summary>
    Task<Gateway> UpdateGatewayAsync(Guid id, string name, GatewayStatus status);

    /// <summary>
    /// Delete gateway
    /// </summary>
    Task<bool> DeleteGatewayAsync(Guid id);

    /// <summary>
    /// Get gateways count
    /// </summary>
    Task<int> GetGatewayCountAsync();
}
