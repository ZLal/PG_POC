using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;
using PaymentGatewayPOC.Services.Interfaces;

namespace PaymentGatewayPOC.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(IUnitOfWork unitOfWork, ILogger<TransactionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all transactions");
            var transactions = await _unitOfWork.Transactions.GetAllAsync();
            _logger.LogInformation($"Retrieved {transactions.Count()} transactions");
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all transactions");
            throw;
        }
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation($"Fetching transaction with ID: {id}");
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogWarning($"Transaction with ID {id} not found");
            }
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching transaction with ID {id}");
            throw;
        }
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByApplicationAsync(Guid applicationId)
    {
        try
        {
            _logger.LogInformation($"Fetching transactions for application ID: {applicationId}");
            var transactions = await _unitOfWork.Transactions.FindAsync(t => t.ApplicationId == applicationId);
            _logger.LogInformation($"Retrieved {transactions.Count()} transactions for application {applicationId}");
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching transactions for application {applicationId}");
            throw;
        }
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByGatewayAsync(Guid gatewayId)
    {
        try
        {
            _logger.LogInformation($"Fetching transactions for gateway ID: {gatewayId}");
            var transactions = await _unitOfWork.Transactions.FindAsync(t => t.GatewayId == gatewayId);
            _logger.LogInformation($"Retrieved {transactions.Count()} transactions for gateway {gatewayId}");
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching transactions for gateway {gatewayId}");
            throw;
        }
    }

    public async Task<Transaction> CreateTransactionAsync(Guid applicationId, Guid gatewayId, decimal amount, string status = "Pending")
    {
        try
        {
            _logger.LogInformation($"Creating new transaction for application ID: {applicationId}, gateway ID: {gatewayId}, amount: {amount}");
            
            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                ApplicationId = applicationId,
                GatewayId = gatewayId,
                Amount = amount,
                Status = status,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Transaction created successfully with ID: {transaction.TransactionId}");
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating transaction for application {applicationId}");
            throw;
        }
    }

    public async Task<Transaction> UpdateTransactionStatusAsync(Guid id, string status)
    {
        try
        {
            _logger.LogInformation($"Updating transaction status with ID: {id}, new status: {status}");
            
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogWarning($"Transaction with ID {id} not found for update");
                throw new KeyNotFoundException($"Transaction with ID {id} not found");
            }

            transaction.Status = status;
            await _unitOfWork.Transactions.UpdateAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Transaction with ID {id} status updated successfully");
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating transaction with ID {id}");
            throw;
        }
    }

    public async Task<TransactionDetail> AddTransactionDetailAsync(Guid transactionId, string status, string? message, string? data)
    {
        try
        {
            _logger.LogInformation($"Adding transaction detail for transaction ID: {transactionId}");
            
            var detail = new TransactionDetail
            {
                TransactionDetailId = Guid.NewGuid(),
                TransactionId = transactionId,
                Status = status,
                Message = message,
                Data = data,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.TransactionDetails.AddAsync(detail);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Transaction detail added successfully with ID: {detail.TransactionDetailId}");
            return detail;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding transaction detail for transaction {transactionId}");
            throw;
        }
    }

    public async Task<ErrorLog> AddErrorLogAsync(Guid? transactionId, string errorMessage)
    {
        try
        {
            _logger.LogError($"Adding error log for transaction ID: {transactionId}, message: {errorMessage}");
            
            var errorLog = new ErrorLog
            {
                LogId = Guid.NewGuid(),
                TransactionId = transactionId,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.ErrorLogs.AddAsync(errorLog);
            await _unitOfWork.SaveChangesAsync();

            return errorLog;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding error log");
            throw;
        }
    }

    public async Task<IEnumerable<TransactionDetail>> GetTransactionDetailsAsync(Guid transactionId)
    {
        try
        {
            _logger.LogInformation($"Fetching details for transaction ID: {transactionId}");
            var details = await _unitOfWork.TransactionDetails.FindAsync(td => td.TransactionId == transactionId);
            _logger.LogInformation($"Retrieved {details.Count()} details for transaction {transactionId}");
            return details;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching transaction details for transaction {transactionId}");
            throw;
        }
    }

    public async Task<IEnumerable<ErrorLog>> GetErrorLogsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all error logs");
            var errorLogs = await _unitOfWork.ErrorLogs.GetAllAsync();
            _logger.LogInformation($"Retrieved {errorLogs.Count()} error logs");
            return errorLogs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all error logs");
            throw;
        }
    }

    public async Task<int> GetTransactionCountAsync()
    {
        try
        {
            _logger.LogInformation("Fetching transaction count");
            var count = await _unitOfWork.Transactions.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction count");
            throw;
        }
    }
}
