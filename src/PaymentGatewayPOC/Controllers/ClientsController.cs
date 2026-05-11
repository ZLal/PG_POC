using Microsoft.AspNetCore.Mvc;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientService clientService, ILogger<ClientsController> logger)
    {
        _clientService = clientService;
        _logger = logger;
    }

    /// <summary>
    /// Get all clients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Client>>> GetAllClients()
    {
        try
        {
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving clients");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving clients");
        }
    }

    /// <summary>
    /// Get client by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Client>> GetClientById(Guid id)
    {
        try
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
            {
                return NotFound($"Client with ID {id} not found");
            }
            return Ok(client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving client with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the client");
        }
    }

    /// <summary>
    /// Get clients by organization ID
    /// </summary>
    [HttpGet("organization/{organizationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByOrganization(Guid organizationId)
    {
        try
        {
            var clients = await _clientService.GetClientsByOrganizationAsync(organizationId);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving clients for organization {organizationId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving clients");
        }
    }

    /// <summary>
    /// Get clients by application ID
    /// </summary>
    [HttpGet("application/{applicationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Client>>> GetClientsByApplication(Guid applicationId)
    {
        try
        {
            var clients = await _clientService.GetClientsByApplicationAsync(applicationId);
            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving clients for application {applicationId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving clients");
        }
    }

    /// <summary>
    /// Create a new client
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Client>> CreateClient([FromBody] CreateClientRequest request)
    {
        try
        {
            if (request.OrganizationId == Guid.Empty || request.ApplicationId == Guid.Empty || 
                string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.SecretKey))
            {
                return BadRequest("OrganizationId, ApplicationId, Name, and SecretKey are required");
            }

            var client = await _clientService.CreateClientAsync(request.OrganizationId, request.ApplicationId, request.Name, request.SecretKey, request.ExpiryDate);
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the client");
        }
    }

    /// <summary>
    /// Update client
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Client>> UpdateClient(Guid id, [FromBody] UpdateClientRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required");
            }

            var client = await _clientService.UpdateClientAsync(id, request.Name, request.ExpiryDate);
            return Ok(client);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Client with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating client with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the client");
        }
    }

    /// <summary>
    /// Delete client
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
            {
                return NotFound($"Client with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting client with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the client");
        }
    }

    /// <summary>
    /// Validate client secret
    /// </summary>
    [HttpPost("{id}/validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> ValidateClientSecret(Guid id, [FromBody] ValidateClientSecretRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.SecretKey))
            {
                return BadRequest("SecretKey is required");
            }

            var isValid = await _clientService.ValidateClientSecretAsync(id, request.SecretKey);
            return Ok(isValid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error validating secret for client {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while validating the client secret");
        }
    }

    /// <summary>
    /// Get client count
    /// </summary>
    [HttpGet("count/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetClientCount()
    {
        try
        {
            var count = await _clientService.GetClientCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving client count");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the client count");
        }
    }
}

public class CreateClientRequest
{
    public Guid OrganizationId { get; set; }
    public Guid ApplicationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
}

public class UpdateClientRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
}

public class ValidateClientSecretRequest
{
    public string SecretKey { get; set; } = string.Empty;
}
