using System;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class GatewaysController : ControllerBase
{
    private readonly IGatewayService _gatewayService;
    private readonly ILogger<GatewaysController> _logger;

    public GatewaysController(IGatewayService gatewayService, ILogger<GatewaysController> logger)
    {
        _gatewayService = gatewayService;
        _logger = logger;
    }

    /// <summary>
    /// Get all gateways
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Gateway>>> GetAllGateways()
    {
        try
        {
            var gateways = await _gatewayService.GetAllGatewaysAsync();
            return Ok(gateways);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving gateways");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving gateways");
        }
    }

    /// <summary>
    /// Get gateway by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Gateway>> GetGatewayById(Guid id)
    {
        try
        {
            var gateway = await _gatewayService.GetGatewayByIdAsync(id);
            if (gateway == null)
            {
                return NotFound($"Gateway with ID {id} not found");
            }
            return Ok(gateway);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving gateway with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the gateway");
        }
    }

    /// <summary>
    /// Get active gateways only
    /// </summary>
    [HttpGet("active/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Gateway>>> GetActiveGateways()
    {
        try
        {
            var gateways = await _gatewayService.GetActiveGatewaysAsync();
            return Ok(gateways);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active gateways");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving active gateways");
        }
    }

    /// <summary>
    /// Create a new gateway
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Gateway>> CreateGateway([FromBody] CreateGatewayRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Gateway name is required");
            }

            var gateway = await _gatewayService.CreateGatewayAsync(request.Name, request.Status);
            return CreatedAtAction(nameof(GetGatewayById), new { id = gateway.GatewayId }, gateway);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating gateway");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the gateway");
        }
    }

    /// <summary>
    /// Update gateway
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Gateway>> UpdateGateway(Guid id, [FromBody] UpdateGatewayRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            var gateway = await _gatewayService.UpdateGatewayAsync(id, request.Name, request.Status);
            return Ok(gateway);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Gateway with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating gateway with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the gateway");
        }
    }

    /// <summary>
    /// Delete gateway
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGateway(Guid id)
    {
        try
        {
            var result = await _gatewayService.DeleteGatewayAsync(id);
            if (!result)
            {
                return NotFound($"Gateway with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting gateway with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the gateway");
        }
    }

    /// <summary>
    /// Get gateway count
    /// </summary>
    [HttpGet("count/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetGatewayCount()
    {
        try
        {
            var count = await _gatewayService.GetGatewayCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving gateway count");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the gateway count");
        }
    }
}

public class CreateGatewayRequest
{
    public string Name { get; set; } = string.Empty;
    public GatewayStatus Status { get; set; } = GatewayStatus.Active;
}

public class UpdateGatewayRequest
{
    public string Name { get; set; } = string.Empty;
    public GatewayStatus Status { get; set; } = GatewayStatus.Active;
}
