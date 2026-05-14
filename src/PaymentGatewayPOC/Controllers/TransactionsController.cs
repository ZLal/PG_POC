using Microsoft.AspNetCore.Mvc;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    /// <summary>
    /// Get all transactions
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
    {
        try
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving transactions");
        }
    }

    /// <summary>
    /// Get transaction by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Transaction>> GetTransactionById(Guid id)
    {
        try
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound($"Transaction with ID {id} not found");
            }
            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving transaction with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the transaction");
        }
    }

    /// <summary>
    /// Get transactions by application ID
    /// </summary>
    [HttpGet("application/{applicationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByApplication(Guid applicationId)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByApplicationAsync(applicationId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving transactions for application {applicationId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving transactions");
        }
    }

    /// <summary>
    /// Get transactions by gateway ID
    /// </summary>
    [HttpGet("gateway/{gatewayId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByGateway(Guid gatewayId)
    {
        try
        {
            var transactions = await _transactionService.GetTransactionsByGatewayAsync(gatewayId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving transactions for gateway {gatewayId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving transactions");
        }
    }

    /// <summary>
    /// Create a new transaction
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        try
        {
            if (request.ApplicationId == Guid.Empty || request.GatewayId == Guid.Empty || request.Amount <= 0)
            {
                return BadRequest("ApplicationId, GatewayId, and positive Amount are required");
            }

            var status = request.Status ?? TransactionStatus.Pending;
            var transaction = await _transactionService.CreateTransactionAsync(request.ApplicationId, request.GatewayId, request.Amount, status);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.TransactionId }, transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the transaction");
        }
    }

    /// <summary>
    /// Update transaction status
    /// </summary>
    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Transaction>> UpdateTransactionStatus(Guid id, [FromBody] UpdateTransactionStatusRequest request)
    {
        try
        {
            var transaction = await _transactionService.UpdateTransactionStatusAsync(id, request.Status);
            return Ok(transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Transaction with ID {id} not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating transaction with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the transaction");
        }
    }

    /// <summary>
    /// Add transaction detail
    /// </summary>
    [HttpPost("{transactionId}/details")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionDetail>> AddTransactionDetail(Guid transactionId, [FromBody] AddTransactionDetailRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest("Status is required");
            }

            var detail = await _transactionService.AddTransactionDetailAsync(transactionId, request.Status, request.Message, request.Data);
            return CreatedAtAction(nameof(GetTransactionDetailsAsync), new { transactionId }, detail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding transaction detail for transaction {transactionId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding transaction detail");
        }
    }

    /// <summary>
    /// Get transaction details
    /// </summary>
    [HttpGet("{transactionId}/details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TransactionDetail>>> GetTransactionDetailsAsync(Guid transactionId)
    {
        try
        {
            var details = await _transactionService.GetTransactionDetailsAsync(transactionId);
            return Ok(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving details for transaction {transactionId}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving transaction details");
        }
    }

    /// <summary>
    /// Get error logs
    /// </summary>
    [HttpGet("errors/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ErrorLog>>> GetErrorLogs()
    {
        try
        {
            var errorLogs = await _transactionService.GetErrorLogsAsync();
            return Ok(errorLogs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving error logs");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving error logs");
        }
    }

    /// <summary>
    /// Add error log
    /// </summary>
    [HttpPost("errors")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ErrorLog>> AddErrorLog([FromBody] AddErrorLogRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.ErrorMessage))
            {
                return BadRequest("ErrorMessage is required");
            }

            var errorLog = await _transactionService.AddErrorLogAsync(request.TransactionId, request.ErrorMessage);
            return CreatedAtAction(nameof(GetErrorLogs), errorLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding error log");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding error log");
        }
    }

    /// <summary>
    /// Get transaction count
    /// </summary>
    [HttpGet("count/total")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetTransactionCount()
    {
        try
        {
            var count = await _transactionService.GetTransactionCountAsync();
            return Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transaction count");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the transaction count");
        }
    }
}

public class CreateTransactionRequest
{
    public Guid ApplicationId { get; set; }
    public Guid GatewayId { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus? Status { get; set; }
}

public class UpdateTransactionStatusRequest
{
    public TransactionStatus Status { get; set; }
}

public class AddTransactionDetailRequest
{
    public string Status { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string? Data { get; set; }
}

public class AddErrorLogRequest
{
    public Guid? TransactionId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
