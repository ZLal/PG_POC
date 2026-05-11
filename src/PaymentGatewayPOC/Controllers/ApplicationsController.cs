using Microsoft.AspNetCore.Mvc;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IApplicationService applicationService, ILogger<ApplicationsController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all applications
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Application>>> GetAllApplications()
    {
        try
        {
            var applications = await _applicationService.GetAllApplicationsAsync();
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving applications");
        }
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Application>> GetApplicationById(Guid id)
    {
        try
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);
            if (application == null)
            {
                return NotFound($"Application with ID {id} not found");
            }
            return Ok(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving application with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the application");
        }
    }

    /// <summary>
    /// Get applications by organization ID
    /// </summary>
    [HttpGet("organization/{organizationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Application>>> GetApplicationsByOrganization(Guid organizationId)
    {
        try
        {
            var applications = await _applicationService.GetApplicationsByOrganizationAsync(organizationId);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving applications for organization {organizationId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving applications");
        }
    }

    /// <summary>
    /// Create a new application
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Application>> CreateApplication([FromBody] CreateApplicationRequest request)
    {
        try
        {
            if (request.OrganizationId == Guid.Empty || string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.AccessLocation))
            {
                return BadRequest("OrganizationId, ClientId, and AccessLocation are required");
            }

            var application = await _applicationService.CreateApplicationAsync(request.OrganizationId, request.ClientId, request.AccessLocation);
            return CreatedAtAction(nameof(GetApplicationById), new { id = application.ApplicationId }, application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the application");
        }
    }

    /// <summary>
    /// Update application
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Application>> UpdateApplication(Guid id, [FromBody] UpdateApplicationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.AccessLocation))
            {
                return BadRequest("ClientId and AccessLocation are required");
            }

            var application = await _applicationService.UpdateApplicationAsync(id, request.ClientId, request.AccessLocation);
            return Ok(application);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Application with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating application with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the application");
        }
    }

    /// <summary>
    /// Delete application
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteApplication(Guid id)
    {
        try
        {
            var result = await _applicationService.DeleteApplicationAsync(id);
            if (!result)
            {
                return NotFound($"Application with ID {id} not found");
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting application with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the application");
        }
    }

    /// <summary>
    /// Get application count
    /// </summary>
    [HttpGet("count/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetApplicationCount()
    {
        try
        {
            var count = await _applicationService.GetApplicationCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application count");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the application count");
        }
    }
}

public class CreateApplicationRequest
{
    public Guid OrganizationId { get; set; }
    public string ClientId { get; set; } = string.Empty;
    public string AccessLocation { get; set; } = string.Empty;
}

public class UpdateApplicationRequest
{
    public string ClientId { get; set; } = string.Empty;
    public string AccessLocation { get; set; } = string.Empty;
}
