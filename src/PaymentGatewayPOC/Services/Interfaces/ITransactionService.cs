using PaymentGatewayPOC.Models;

namespace PaymentGatewayPOC.Services.Interfaces;

public interface ITransactionService
{
    /// <summary>
    /// Get all transactions
    /// </summary>
    Task<IEnumerable<Transaction>> GetAllTransactionsAsync();

    /// <summary>
    /// Get transaction by ID
    /// </summary>
    Task<Transaction?> GetTransactionByIdAsync(Guid id);

    /// <summary>
    /// Get transactions by application ID
    /// </summary>
    Task<IEnumerable<Transaction>> GetTransactionsByApplicationAsync(Guid applicationId);

    /// <summary>
    /// Get transactions by gateway ID
    /// </summary>
    Task<IEnumerable<Transaction>> GetTransactionsByGatewayAsync(Guid gatewayId);

    /// <summary>
    /// Create a new transaction
    /// </summary>
    Task<Transaction> CreateTransactionAsync(Guid applicationId, Guid gatewayId, decimal amount, TransactionStatus status);

    /// <summary>
    /// Update transaction status
    /// </summary>
    Task<Transaction> UpdateTransactionStatusAsync(Guid id, TransactionStatus status);

    /// <summary>
    /// Add transaction detail/log
    /// </summary>
    Task<TransactionDetail> AddTransactionDetailAsync(Guid transactionId, string status, string? message, string? data);

    /// <summary>
    /// Add error log for transaction
    /// </summary>
    Task<ErrorLog> AddErrorLogAsync(Guid? transactionId, string errorMessage);

    /// <summary>
    /// Get transaction details
    /// </summary>
    Task<IEnumerable<TransactionDetail>> GetTransactionDetailsAsync(Guid transactionId);

    /// <summary>
    /// Get error logs
    /// </summary>
    Task<IEnumerable<ErrorLog>> GetErrorLogsAsync();

    /// <summary>
    /// Get transactions count
    /// </summary>
    Task<int> GetTransactionCountAsync();
}
