using Microsoft.EntityFrameworkCore.Storage;
using PaymentGatewayPOC.Data;
using PaymentGatewayPOC.Models;
using PaymentGatewayPOC.Repositories.Interfaces;

namespace PaymentGatewayPOC.Repositories;

/// <summary>
/// Unit of Work implementation to manage all repositories and database transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly PaymentGatewayContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Organization>? _organizationRepository;
    private IRepository<Application>? _applicationRepository;
    private IRepository<Client>? _clientRepository;
    private IRepository<Gateway>? _gatewayRepository;
    private IRepository<Transaction>? _transactionRepository;
    private IRepository<TransactionDetail>? _transactionDetailRepository;
    private IRepository<ErrorLog>? _errorLogRepository;

    public UnitOfWork(PaymentGatewayContext context)
    {
        _context = context;
    }

    public IRepository<Organization> Organizations =>
        _organizationRepository ??= new Repository<Organization>(_context);

    public IRepository<Application> Applications =>
        _applicationRepository ??= new Repository<Application>(_context);

    public IRepository<Client> Clients =>
        _clientRepository ??= new Repository<Client>(_context);

    public IRepository<Gateway> Gateways =>
        _gatewayRepository ??= new Repository<Gateway>(_context);

    public IRepository<Transaction> Transactions =>
        _transactionRepository ??= new Repository<Transaction>(_context);

    public IRepository<TransactionDetail> TransactionDetails =>
        _transactionDetailRepository ??= new Repository<TransactionDetail>(_context);

    public IRepository<ErrorLog> ErrorLogs =>
        _errorLogRepository ??= new Repository<ErrorLog>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await SaveChangesAsync();
            await _transaction?.CommitAsync()!;
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _transaction?.DisposeAsync()!;
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        try
        {
            await _transaction?.RollbackAsync()!;
        }
        finally
        {
            await _transaction?.DisposeAsync()!;
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
