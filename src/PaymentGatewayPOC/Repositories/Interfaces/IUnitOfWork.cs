namespace PaymentGatewayPOC.Repositories.Interfaces;

/// <summary>
/// Unit of Work pattern interface to manage all repositories
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<Models.Organization> Organizations { get; }
    IApplicationRepository Applications { get; }
    IRepository<Models.Client> Clients { get; }
    IRepository<Models.Gateway> Gateways { get; }
    IRepository<Models.ApplicationGateway> ApplicationGateways { get; }
    IRepository<Models.Transaction> Transactions { get; }
    IRepository<Models.TransactionDetail> TransactionDetails { get; }
    IRepository<Models.ErrorLog> ErrorLogs { get; }

    /// <summary>
    /// Save all changes to the database
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begin a database transaction
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commit the current transaction
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rollback the current transaction
    /// </summary>
    Task RollbackTransactionAsync();
}
