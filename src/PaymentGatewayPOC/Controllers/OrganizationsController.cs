using Microsoft.AspNetCore.Mvc;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly ILogger<OrganizationsController> _logger;

    public OrganizationsController(IOrganizationService organizationService, ILogger<OrganizationsController> logger)
    {
        _organizationService = organizationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all organizations
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Organization>>> GetAllOrganizations()
    {
        try
        {
            var organizations = await _organizationService.GetAllOrganizationsAsync();
            return Ok(organizations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving organizations");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving organizations");
        }
    }

    /// <summary>
    /// Get organization by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Organization>> GetOrganizationById(Guid id)
    {
        try
        {
            var organization = await _organizationService.GetOrganizationByIdAsync(id);
            if (organization == null)
            {
                return NotFound($"Organization with ID {id} not found");
            }
            return Ok(organization);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving organization with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the organization");
        }
    }

    /// <summary>
    /// Create a new organization
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Organization>> CreateOrganization([FromBody] CreateOrganizationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Organization name is required");
            }

            var organization = await _organizationService.CreateOrganizationAsync(request.Name);
            return CreatedAtAction(nameof(GetOrganizationById), new { id = organization.OrganizationId }, organization);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating organization");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the organization");
        }
    }

    /// <summary>
    /// Update organization
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Organization>> UpdateOrganization(Guid id, [FromBody] UpdateOrganizationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Organization name is required");
            }

            var organization = await _organizationService.UpdateOrganizationAsync(id, request.Name);
            return Ok(organization);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Organization with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating organization with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the organization");
        }
    }

    /// <summary>
    /// Delete organization
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrganization(Guid id)
    {
        try
        {
            var result = await _organizationService.DeleteOrganizationAsync(id);
            if (!result)
            {
                return NotFound($"Organization with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting organization with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the organization");
        }
    }

    /// <summary>
    /// Get organization count
    /// </summary>
    [HttpGet("count/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetOrganizationCount()
    {
        try
        {
            var count = await _organizationService.GetOrganizationCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving organization count");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the organization count");
        }
    }
}

public class CreateOrganizationRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateOrganizationRequest
{
    public string Name { get; set; } = string.Empty;
}
